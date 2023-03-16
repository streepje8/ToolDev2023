using System;
using System.Runtime.InteropServices;
using TMPro;

//Used code from https://www.dunnies-it.com/unity/open-file-dialog-script-in-unity.php because without it i would never figure this out. (Yes i know i could have used the standalone file browser but whats the fun in that)
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName {
    public int      structSize = 0;
    public IntPtr   dlgOwner = IntPtr.Zero;
    public IntPtr   instance = IntPtr.Zero;
    public String   filter = null;
    public String   customFilter = null;
    public int      maxCustFilter = 0;
    public int      filterIndex = 0;
    public String   file = null;
    public int      maxFile = 0;
    public String   fileTitle = null;
    public int      maxFileTitle = 0;
    public String   initialDir = null;
    public String   title = null;
    public int      flags = 0;
    public short    fileOffset = 0;
    public short    fileExtension = 0;
    public String   defExt = null;
    public IntPtr   custData = IntPtr.Zero;
    public IntPtr   hook = IntPtr.Zero;
    public String   templateName = null;
    public IntPtr   reservedPtr = IntPtr.Zero;
    public int      reservedInt = 0;
    public int      flagsEx = 0;
}

public class FileInputElement : MenuElement
{
    public string FilePath { get; private set; } = null;
    public TMP_Text previewText;
    
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn) {
        return GetOpenFileName(ofn);
    }
    
    public void PickFile()
    {
        
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "Sprites \0*.png\0\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = "%USERPROFILE%\\Desktop";
        ofn.title = "Select an png file with a sprite to use for your project.";
        ofn.defExt = "PNG";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST|OFN_NOCHANGEDIR
        if(GetOpenFileName(ofn))
        {
            FilePath = ofn.file;
        }
        else
        {
            FilePath = null;
        }
        UpdatePreviewText();
    }

    public void UpdatePreviewText()
    {
        previewText.text = FilePath ?? "No File Selected";
    }
}
