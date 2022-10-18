using System.Collections.Generic;
using UnityEngine;

namespace DunnGSunn
{
    public class SunUniCipher : MonoBehaviour
    {
        private static SunUniCipher _instance;

        public static SunUniCipher Instance
        {
            get
            {
                if (_instance == null)
                {
                    var singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<SunUniCipher>();
                    singletonObject.name = "Sun UniCipher";
                    Debug.Log("Create Sun UniCipher.");
                }

                return _instance;
            }
        }

        public string separator = "@";
        private List<string> definitions = new List<string>();

        private void Awake()
        {
            if (_instance != null && _instance.GetInstanceID() != GetInstanceID())
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            definitions.Add("a-jkhd");
            definitions.Add("A-htr4");
            definitions.Add("b-4fdd");
            definitions.Add("B-4hfd");
            definitions.Add("c-980g");
            definitions.Add("C-99fg");
            definitions.Add("d-98fr");
            definitions.Add("D-ffb4");
            definitions.Add("e-njhk");
            definitions.Add("E-dsh4");
            definitions.Add("f-njhk");
            definitions.Add("F-x8cx");
            definitions.Add("g-jhhy");
            definitions.Add("G-j56h");
            definitions.Add("h-jkhu");
            definitions.Add("H-j9fd");
            definitions.Add("i-978f");
            definitions.Add("I-764r");
            definitions.Add("j-jnoi");
            definitions.Add("J-9f8g");
            definitions.Add("k-nhgf");
            definitions.Add("K-fdjh");
            definitions.Add("l-juy5");
            definitions.Add("L-fd9f");
            definitions.Add("m-jhki");
            definitions.Add("M-fd98");
            definitions.Add("n-h78d");
            definitions.Add("N-mkol");
            definitions.Add("o-980f");
            definitions.Add("O-poj0");
            definitions.Add("p-9fee");
            definitions.Add("P-7yhg");
            definitions.Add("q-jhgf");
            definitions.Add("Q-jf56");
            definitions.Add("r-kocv");
            definitions.Add("R-jium");
            definitions.Add("s-9ugf");
            definitions.Add("S-klfs");
            definitions.Add("t-lkgd");
            definitions.Add("T-09uj");
            definitions.Add("u-bfhg");
            definitions.Add("U-76fg");
            definitions.Add("v-234f");
            definitions.Add("V-098g");
            definitions.Add("w-hgnf");
            definitions.Add("W-0976");
            definitions.Add("x-75i8");
            definitions.Add("X-876l");
            definitions.Add("y-vfd8");
            definitions.Add("Y-klnh");
            definitions.Add("z-9gwe");
            definitions.Add("Z-kljo");
            definitions.Add("1-gfnj");
            definitions.Add("2-mjhf");
            definitions.Add("3-dwev");
            definitions.Add("4-ogjh");
            definitions.Add("5-mbhg");
            definitions.Add("6-pfjn");
            definitions.Add("7-9fjn");
            definitions.Add("8-76yf");
            definitions.Add("9-nmf9");
            definitions.Add("0-3nfp");
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public string Encrypt(string text)
        {
            var result = "";
            foreach (var c in text)
            {
                result += CharEncrypt(c);
            }

            return result;
        }

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

        public string CharEncrypt(char c)
        {
            foreach (var definition in definitions)
            {
                if (char.Parse(definition.Split('-')[0]) == c)
                {
                    return definition.Split('-')[1] + separator;
                }
            }

            return c + separator;
        }

        public string CharDecrypt(string part)
        {
            foreach (var definition in definitions)
            {
                if (definition.Split('-')[1] == part)
                {
                    return definition.Split('-')[0];
                }
            }

            return part;
        }
    }
}