using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Http;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Android;
using UnityEngine.Networking;


public class mainScript : MonoBehaviour
{

    public TMP_Text question;
    public TMP_Text isCorrect;

    public TMP_Text respuesta1;
    public TMP_Text respuesta2;
    public TMP_Text respuesta3;
    public TMP_Text respuesta4;

    public Button b0;
    public Button b1;
    public Button b2;
    public Button b3;

    public Button next;

    private QuestionResponse qr;

    private string currentQuestion;

    // Start is called before the first frame update
    void Start()
    {
        isCorrect.text = "";
        next.interactable = false;

        question.text = "Cargando pregunta un momento...";
        respuesta1.text = "Cargando...";
        respuesta2.text = "Cargando...";
        respuesta3.text = "Cargando...";
        respuesta4.text = "Cargando...";
        // var handler = new HttpClientHandler()
        // {
        //     SslProtocols = SslProtocols.None
        // };        
        // string jsonString = await getQuestion(new HttpClient());
        StartCoroutine(getQuestion());
        currentQuestion = "";
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator getQuestion()
    {
        string url = "https://192.168.1.58:6312/get_new_question";
        // var httpClient = ;
        
        question.text = "Esperando respuesta del servidor...";

        SelfCertificateHandler certHandler = new SelfCertificateHandler();

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.certificateHandler = certHandler;
        
        yield return www.SendWebRequest();
 
        if(www.responseCode != 200) {
            Debug.Log(www.error);
        }
        else {
            // // Show results as text
            // Debug.Log(www.downloadHandler.text);
 
            // Or retrieve results as binary data
            // currentQuestion = www.downloadHandler.text;
            qr = QuestionResponse.CreateFromJSON(www.downloadHandler.text);
        
            question.text = qr.question;
            respuesta1.text = qr.option0;
            respuesta2.text = qr.option1;
            respuesta3.text = qr.option2;
            respuesta4.text = qr.option3;
        }
    }

    public void choose(int option)
    {
        ColorBlock green = ColorBlock.defaultColorBlock;
        green.pressedColor = Color.green;
        green.highlightedColor = Color.green;
        green.normalColor = Color.green;
        green.selectedColor = Color.green;
        switch (qr.solution)
        {
            case 0:
                b0.colors = green;

                break;
            case 1:
                b1.colors = green;

                break;
            case 2:
                b2.colors = green;

                break;
            case 3:
                b3.colors = green;
  
                break;
        }
        
        isCorrect.text = "Ehhh...";
        if (option == qr.solution)
        {
            isCorrect.text = "Correcto";
        }
        else
        {
            isCorrect.text = "Incorrecto";
        }
        green.pressedColor = Color.green;
        green.highlightedColor = Color.green;
        green.normalColor = Color.green;
        next.interactable = true;
    }

    public void Next()
    {
        ColorBlock def = ColorBlock.defaultColorBlock;

        b0.colors = def;
        b1.colors = def;
        b2.colors = def;
        b3.colors = def;

        isCorrect.text = "";
        next.interactable = false;

        question.text = "Cargando pregunta un momento...";
        respuesta1.text = "Cargando...";
        respuesta2.text = "Cargando...";
        respuesta3.text = "Cargando...";
        respuesta4.text = "Cargando...";

        StartCoroutine(getQuestion());
        qr = QuestionResponse.CreateFromJSON(currentQuestion);
        
        question.text = qr.question;
        respuesta1.text = qr.option0;
        respuesta2.text = qr.option1;
        respuesta3.text = qr.option2;
        respuesta4.text = qr.option3;
    }
}

[System.Serializable]
public class QuestionResponse
{
    public string question;
    public string option0;
    public string option1;
    public string option2;
    public string option3;
    public int solution;

    public static QuestionResponse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<QuestionResponse>(jsonString);
    }
}

public class SelfCertificateHandler : CertificateHandler
{
    // Encoded RSAPublicKey
    private static string PUB_KEY = "3082020A0282020100CC57F45AAC0FD0D1DDC70C8DDC390922487673949FF8956015E69861B82219A67CE7810D9A173F447AF928332A2D55954859C646951FE4D97A6C47EF09AE1C4A806EB4FCD79539522F94F090335CEF05A30B2F9CF3142EC1ED93ED25AFADE07705A1E682646B5D41817B7A72EC050BAF4BDA7022DB979278CE338455F53BAD87D801F66DC2D4F698461875DF873319C232FCE9FBB69E18CAA370DB791AE991B0CDD2315050DA13B44E62CBA38BFA960A9C62AFA29786687E395C244B1F24D14356D53E2147C64669D5D37AD0610AE7629551381528752804189B1F47BDFCE1D1B096DAD2B1701D5AD349A1E728865178B749C0354904753438A255B53D41C6F90F3532C77733E27F05B0705059DF0A74F5A6F27DC2575CDA1002B73B744924867AC08819D9C38F50CA573EA9E3E57C878A40624784B28168ED934C7493EF22A77D2502E0880ED1B0310C654BE65C03E2A719A0DDC4B70FE80CC765D2DA964F7E88D759A08975A02406A66C6743F414A33EB6039C9283D68E11C55E4A3D3F2119A258617F1E93A7A0E1A4231EDE8BFEB0AE30E1A7E4AD26D18DE67C346643BA2C1A52D7C3219F0E02521911E86C7CEA672432F939779BFD9C7439EB8074190B5C862E634A11156E480F978E5B4EF3B5064C826BE9B6A18BE2C84507CFC9FC0315A78311290B89FAAB7E1ABAA24C1C1ED19C1BD8D519F6C1645F81C33C7E1277AF0203010001";

    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        Debug.Log(pk);
        if (pk.Equals(PUB_KEY))
            return true;

        // Bad dog
        return false;
    }
}
