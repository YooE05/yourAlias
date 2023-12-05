using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextTransfer : MonoBehaviour
{
    [SerializeField]
    private string shortcut;

    string tmpString;


    [ContextMenu("Import")]
    private void ImportText()
    {
        StreamReader inp_stm = new StreamReader(shortcut);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            tmpString+= inp_ln;

        }
        Debug.Log(tmpString);
        inp_stm.Close();
    }

    [ContextMenu("Export")]
    private void ExportText()
    {
        StreamReader inp_stm = new StreamReader(shortcut);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            tmpString += inp_ln;

        }
        Debug.Log(tmpString);
        tmpString = "";
        inp_stm.Close();
    }


}
