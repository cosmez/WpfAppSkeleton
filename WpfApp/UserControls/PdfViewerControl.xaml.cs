using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PDFiumCore;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Size = System.Windows.Size;
using Bitmap = System.Drawing.Bitmap;
using System.Windows.Media.Imaging;

namespace WpfApp.UserControls;
/// <summary>
/// Interaction logic for PdfViewerControl.xaml
/// </summary>
public partial class PdfViewerControl : UserControl
{
    public PdfViewerControl()
    {
        InitializeComponent();
    }

    private FpdfDocumentT? _documentT = null;
    private BitmapSource? _bitmapSource = null;

    public static readonly DependencyProperty FilenameProperty =
        DependencyProperty.Register(nameof(Filename), typeof(string), typeof(PdfViewerControl),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFilenameChanged));

    public string? Filename
    {
        get => (string)GetValue(FilenameProperty);
        set => SetValue(FilenameProperty, value);
    }

    private static void OnFilenameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (PdfViewerControl)d;
        // You can add logic here when the Filename changes
        control.Render();
        control.InvalidateVisual();
    }


    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
         

    }

    public void Close()
    {
        if (_documentT is not null)
        {
            fpdfview.FPDF_CloseDocument(_documentT);
        }
    }

    private void Render()
    {
        if (Filename is not null && File.Exists(Filename))
        {
            Init();
            if (_documentT is not null) fpdfview.FPDF_CloseDocument(_documentT);
            _documentT = fpdfview.FPDF_LoadDocument(Filename, null);
            if (_documentT is null) return;
            var renderSize = GetPageSize(_documentT, 0);
            if (renderSize is null) return;
            using MemoryStream memory = new MemoryStream();
            var bitmap = Render(_documentT, 0, renderSize.Value, 0);
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze(); // Freeze to make it cross-thread accessible
            _bitmapSource = bitmapImage;
            PdfImage.Source = _bitmapSource;
        }
    }

    public static void ReleasePdfium()
    {
        if (_isInitialized)
        {
            fpdfview.FPDF_DestroyLibrary();
        }
    }
    
    private static readonly object PdfLock = new object();
    private static bool _isInitialized ;
    private void Init()
    {
        if (!_isInitialized)
        {

            fpdfview.FPDF_InitLibrary();
            _isInitialized = true;
        }
        //config.Dispose();
    }

    private unsafe Size? GetPageSize(FpdfDocumentT? document, int pageNumber)
    {
        if (document is not null)
        {
            double width = 0, height = 0;
            fpdfview.FPDF_GetPageSizeByIndex(document, pageNumber, ref width, ref height);
            return new Size(width, height);
        }

        return null;
    }

    private unsafe Bitmap Render(FpdfDocumentT document, int pageNumber, Size renderSize, int rotation = 0)
    {
        PixelFormat FpdFtoPixelFormat(int bitmapFormat)
        {
            switch (bitmapFormat)
            {
                case 4:
                    return PixelFormat.Format32bppArgb;
                case 2:
                    return PixelFormat.Format8bppIndexed;
                default:
                    return PixelFormat.Format32bppArgb;
            }

        }
        
        lock (PdfLock)
        {
            //background color
            uint color = uint.MaxValue;
            var page = fpdfview.FPDF_LoadPage(document, pageNumber);
            //get the render size calculated
            var viewport = new Rect()
            {
                X = 0,
                Y = 0,
                Width = renderSize.Width,
                Height = renderSize.Height
            };


            //rotate bitmap accordingly
            var bitmap = fpdfview.FPDFBitmapCreateEx((int)viewport.Width, (int)viewport.Height,
                (int)FPDFBitmapFormat.BGRA, IntPtr.Zero, 0);

            if (bitmap == null) throw new Exception("failed to create a bitmap object");

            fpdfview.FPDFBitmapFillRect(bitmap, 0, 0, (int)viewport.Width, (int)viewport.Height, color);
            var bitmapFormat = fpdfview.FPDFBitmapGetFormat(bitmap);

 
            /////          rotate      -   Page orientation:
            //                            0 (normal)
            //                            1 (rotated 90 degrees clockwise)
            //                            2 (rotated 180 degrees)
            //                            3 (rotated 90 degrees counter-clockwise)


            
            //render to a Pdfium Bitmap first
            fpdfview.FPDF_RenderPageBitmap(bitmap, page, 0, 0, (int)viewport.Width, (int)viewport.Height, rotation,
                (int)RenderFlags.RenderAnnotations);

            //pointer to the internal bitmap data structure
            var scan0 = fpdfview.FPDFBitmapGetBuffer(bitmap);
            var stride = fpdfview.FPDFBitmapGetStride(bitmap);

            // convert to appropiate System.Drawing image type
            var pixelFormat = FpdFtoPixelFormat(bitmapFormat);
            var wfBitmap = new Bitmap((int)viewport.Width, (int)viewport.Height, pixelFormat);
            var rect = new System.Drawing.Rectangle(0, 0, (int)viewport.Width, (int)viewport.Height);
            System.Drawing.Imaging.BitmapData bmpData = wfBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, pixelFormat);

            //Marshal.Copy(scan0, bmpData.Scan0, 0, stride * viewport.Height);
            var ptr1 = (void*)scan0;
            var ptr2 = (void*)bmpData.Scan0;
            long size = stride * (int)viewport.Height;
            //there are many ways to copy native memory. but this one seems to be very common
            //Marshal.Copy(ptr1, ptr2, 0, 10);
            Buffer.MemoryCopy(ptr1, ptr2, size, size);
            //CopyMemory(ptr1, ptr2, (uint)(stride * viewport.Height));
            //Marshal.Copy(ptr1, ptr2, (uint)(stride * viewport.Height));

            wfBitmap.UnlockBits(bmpData);


            fpdfview.FPDFBitmapDestroy(bitmap);
            Marshal.FreeHGlobal(scan0);

            fpdfview.FPDF_ClosePage(page);

            
            //i dont think this is necessary
            GC.Collect();

            return wfBitmap;
        }
    }
}
