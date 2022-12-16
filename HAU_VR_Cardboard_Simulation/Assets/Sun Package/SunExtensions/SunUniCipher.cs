using System.Collections.Generic;
using UnityEngine;

namespace Sun_Package
{
    public class SunUniCipher : SunMonoSingleton<SunUniCipher>
    {
        #region Variables

        //
        [SerializeField] private string separator = "@";
        
        //
        private List<string> _definitions = new List<string>();

        #endregion

        #region Functions

        //
        private void Start()
        {
            _definitions.Add("a-jkhd");
            _definitions.Add("A-htr4");
            _definitions.Add("b-4fdd");
            _definitions.Add("B-4hfd");
            _definitions.Add("c-980g");
            _definitions.Add("C-99fg");
            _definitions.Add("d-98fr");
            _definitions.Add("D-ffb4");
            _definitions.Add("e-njhk");
            _definitions.Add("E-dsh4");
            _definitions.Add("f-njhk");
            _definitions.Add("F-x8cx");
            _definitions.Add("g-jhhy");
            _definitions.Add("G-j56h");
            _definitions.Add("h-jkhu");
            _definitions.Add("H-j9fd");
            _definitions.Add("i-978f");
            _definitions.Add("I-764r");
            _definitions.Add("j-jnoi");
            _definitions.Add("J-9f8g");
            _definitions.Add("k-nhgf");
            _definitions.Add("K-fdjh");
            _definitions.Add("l-juy5");
            _definitions.Add("L-fd9f");
            _definitions.Add("m-jhki");
            _definitions.Add("M-fd98");
            _definitions.Add("n-h78d");
            _definitions.Add("N-mkol");
            _definitions.Add("o-980f");
            _definitions.Add("O-poj0");
            _definitions.Add("p-9fee");
            _definitions.Add("P-7yhg");
            _definitions.Add("q-jhgf");
            _definitions.Add("Q-jf56");
            _definitions.Add("r-kocv");
            _definitions.Add("R-jium");
            _definitions.Add("s-9ugf");
            _definitions.Add("S-klfs");
            _definitions.Add("t-lkgd");
            _definitions.Add("T-09uj");
            _definitions.Add("u-bfhg");
            _definitions.Add("U-76fg");
            _definitions.Add("v-234f");
            _definitions.Add("V-098g");
            _definitions.Add("w-hgnf");
            _definitions.Add("W-0976");
            _definitions.Add("x-75i8");
            _definitions.Add("X-876l");
            _definitions.Add("y-vfd8");
            _definitions.Add("Y-klnh");
            _definitions.Add("z-9gwe");
            _definitions.Add("Z-kljo");
            _definitions.Add("1-gfnj");
            _definitions.Add("2-mjhf");
            _definitions.Add("3-dwev");
            _definitions.Add("4-ogjh");
            _definitions.Add("5-mbhg");
            _definitions.Add("6-pfjn");
            _definitions.Add("7-9fjn");
            _definitions.Add("8-76yf");
            _definitions.Add("9-nmf9");
            _definitions.Add("0-3nfp");
        }
        
        //
        public string Encrypt(string text)
        {
            var result = "";
            foreach (var c in text)
            {
                result += CharEncrypt(c);
            }

            return result;
        }
        
        //
        public string Decrypt(string text)
        {
            var enctext = text.Split(char.Parse(separator));
            var result = "";
            foreach (var part in enctext)
            {
                result += CharDecrypt(part);
            }

            return result;
        }
        
        //
        public string CharEncrypt(char c)
        {
            foreach (var definition in _definitions)
            {
                if (char.Parse(definition.Split('-')[0]) == c)
                {
                    return definition.Split('-')[1] + separator;
                }
            }

            return c + separator;
        }
        
        //
        public string CharDecrypt(string part)
        {
            foreach (var definition in _definitions)
            {
                if (definition.Split('-')[1] == part)
                {
                    return definition.Split('-')[0];
                }
            }

            return part;
        }

        #endregion
    }
}