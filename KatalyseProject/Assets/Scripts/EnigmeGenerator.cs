using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnigmeGenerator : MonoBehaviour
{
    public List<TextMeshPro> ltmpEnigmeCode;
    public ParticleSystem psParticleSystem;
    public TextMeshPro tmpVictoire;
    public TextMeshPro tmpCodeSolv;
    private string sTrueCode;
    private string sTrueString;
    public int iNumberTrueOfCode;
    public int iNumberWrongOfCode;

    Dictionary<int, string> dCode;

    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    void Start()
    {
        GameManager.getInstance().egEnigmeGenerator = this;
        dCode = new Dictionary<int, string>();
        int i = 0;
        foreach (var item in ltmpEnigmeCode)
        {
            item.text = "";
        }
        while (i < iNumberTrueOfCode + iNumberWrongOfCode)
        {
            if (i < iNumberTrueOfCode)
            {
                int tmpcode = GenerateCode();
                sTrueCode += tmpcode + " ";
                dCode.TryGetValue(tmpcode, out string str);

                sTrueString += str;

                TMP_Text text = ltmpEnigmeCode[Random.Range(0, ltmpEnigmeCode.Count - 1)];
                while (text.text != "")
                {
                    text = ltmpEnigmeCode[Random.Range(0, ltmpEnigmeCode.Count - 1)];
                }
                text.text = tmpcode + " - " + str;
                i++;
            }
            else
            {
                int tmpcode = GenerateCode();
                dCode.TryGetValue(GenerateCode(), out string str);

                TMP_Text text = ltmpEnigmeCode[Random.Range(0, ltmpEnigmeCode.Count - 1)];
                while (text.text != "")
                {
                    text = ltmpEnigmeCode[Random.Range(0, ltmpEnigmeCode.Count - 1)];
                }
                text.text = tmpcode + " - " + str;
                i++;
            }
        }

        tmpCodeSolv.text = sTrueCode;

    }

    void Update()
    {
        
    }

    int GenerateCode()
    {
        int Code = Random.Range(1, 12);
        while (dCode.ContainsKey(Code))
        {
            Code = Random.Range(1, 12);
        }
        string FirstLetter = alphabet[Random.Range(0, alphabet.Length - 1)].ToString();
        string SecondLetter = alphabet[Random.Range(0, alphabet.Length - 1)].ToString();
        dCode.Add(Code, FirstLetter + SecondLetter);
        return Code;
    }

    public string GetTrueStringCode()
    {
        return sTrueString.ToLower();
    }

    public void Victory(string victorytext)
    {
        tmpVictoire.SetText(victorytext);
        psParticleSystem.Play();
    }
}
