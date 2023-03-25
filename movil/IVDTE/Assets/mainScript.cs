using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.UI;


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



    // Start is called before the first frame update
    async void Start()
    {
        isCorrect.text = "";
        next.interactable = false;

        question.text = "Cargando pregunta un momento...";
        respuesta1.text = "Cargando...";
        respuesta2.text = "Cargando...";
        respuesta3.text = "Cargando...";
        respuesta4.text = "Cargando...";
        
        string jsonString = await getQuestion(new HttpClient());
        qr = QuestionResponse.CreateFromJSON(jsonString);
        
        question.text = qr.question;
        respuesta1.text = qr.option0;
        respuesta2.text = qr.option1;
        respuesta3.text = qr.option2;
        respuesta4.text = qr.option3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async Task<string> getQuestion(HttpClient httpClient)
    {
        string url = "http://192.168.1.58:5000/get_new_question";
        // var httpClient = ;
        
        question.text = "Esperando respuesta del servidor...";
        var response = await httpClient.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            question.text = "Leyendo respuesta...";
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            question.text = "Ha habido un error al obtener la pregunta.";
            return null;
        }
    }

    public void choose(int option)
    {
        ColorBlock green = ColorBlock.defaultColorBlock;
        green.pressedColor = Color.green;
        green.highlightedColor = Color.green;
        green.normalColor = Color.green;
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

    public async void Next()
    {
        isCorrect.text = "";
        next.interactable = false;

        ColorBlock def = ColorBlock.defaultColorBlock;
        b0.colors = def;
        b1.colors = def;
        b2.colors = def;
        b3.colors = def;


        question.text = "Cargando pregunta un momento...";
        respuesta1.text = "Cargando...";
        respuesta2.text = "Cargando...";
        respuesta3.text = "Cargando...";
        respuesta4.text = "Cargando...";
        
        string jsonString = await getQuestion(new HttpClient());
        qr = QuestionResponse.CreateFromJSON(jsonString);
        
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
