using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;

public class GameManagerActividades : MonoBehaviour
{
    [SerializeField] Button BotonBailar;
    [SerializeField] Button BotonCantar;
    [SerializeField] Button BotonGritar;
    [SerializeField] Button BotonSentadilla;
    [SerializeField] Button BotonLagartija;
    [SerializeField] Button BotonTijera;
    [SerializeField] Text TextContador;
    [SerializeField] GameObject BotonRestart;



    private static GameManagerActividades instance;
    private Thread receiveThread;
    private UdpClient _dataReceiveCiclista;
    private UdpClient _dataReceiveSabotaje;
    private IPEndPoint _receiveEndPointDataCiclista;
    private IPEndPoint _receiveEndPointDataSabotaje;
    public string _ipDataCiclista = "192.168.100.13"; //IP PC
    public string _ipDataSabotaje = "192.168.100.5"; //IP Cell2
    public int Disponible = 3;

    public int _receivePortData = 3100;
    public int _sendPortData = 3500;
    private bool isInitialized;
    private Queue receiveQueue;

    private string _dataReceived;



    void Awake()
    {
        BotonRestart.SetActive(false);

        TextContador.text = (PlayerPrefs.GetInt("ValorDisponible")).ToString();


        Initialize();
        
        if (PlayerPrefs.GetInt("EstadoBotonBailar")==1){
            BotonBailar.enabled = false;
            Debug.Log(PlayerPrefs.GetInt("EstadoBotonBailar"));
        }
        if (PlayerPrefs.GetInt("EstadoBotonCantar") == 1)
        {
            BotonCantar.enabled = false;
        }
        if (PlayerPrefs.GetInt("EstadoBotonGritar") == 1)
        {
            BotonGritar.enabled = false;
        }
        if (PlayerPrefs.GetInt("EstadoBotonLagartija") == 1)
        {
            BotonLagartija.enabled = false;
        }
        if (PlayerPrefs.GetInt("EstadoBotonSentadilla") == 1)
        {
            BotonSentadilla.enabled = false;
        }
        if (PlayerPrefs.GetInt("EstadoBotonTijera") == 1)
        {
            BotonTijera.enabled = false;
        }
        if (PlayerPrefs.GetInt("ValorDisponible") == 3)
        {
            
            ButtonRestar();

        }
       

    }

    private void Initialize()
    {
        instance = this;
        _receiveEndPointDataCiclista = new IPEndPoint(IPAddress.Parse(_ipDataCiclista), _sendPortData);
        _receiveEndPointDataSabotaje = new IPEndPoint(IPAddress.Parse(_ipDataSabotaje), _sendPortData);
        _dataReceiveSabotaje = new UdpClient(_receivePortData);
        receiveQueue = Queue.Synchronized(new Queue());


         receiveThread = new Thread(new ThreadStart(ReceiveDataListener));
         receiveThread.IsBackground = true;
         receiveThread.Start();
        isInitialized = true;
    }

    private void ReceiveDataListener()
    {
        while (true)
        {
            try
            {
                byte[] dataPulse = _dataReceiveSabotaje.Receive(ref _receiveEndPointDataSabotaje);
                receiveQueue.Enqueue(dataPulse);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }

    private void OnDestroy()
    {
        TryKillThread();
    }
    private void OnApplicationQuit()
    {
        TryKillThread();
    }
    private void TryKillThread()
    {
        if (isInitialized)
        {
            receiveThread.Abort();
            receiveThread = null;

            _dataReceiveSabotaje.Close();
            _dataReceiveSabotaje = null;

            Debug.Log("Thread killed");
            isInitialized = false;
        }
    }
    

    private void sendStringDataCiclista(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _dataReceiveCiclista.Send(data, data.Length, _receiveEndPointDataCiclista);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }
    private void sendStringDataSabotaje(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _dataReceiveSabotaje.Send(data, data.Length, _receiveEndPointDataSabotaje);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }

    public void ButtonBailar()
    {
        sendStringDataSabotaje("Bailar");
        PlayerPrefs.SetInt("EstadoBotonBailar", 1);
        PlayerPrefs.Save();
        int tmp = PlayerPrefs.GetInt("ValorDisponible") + 1;
        PlayerPrefs.SetInt("ValorDisponible", tmp);
        PlayerPrefs.Save();
        TryKillThread();
        SceneManager.LoadScene("RevisionMain");
    }
    public void ButtonCantar()
    {
        sendStringDataSabotaje("Cantar");
        PlayerPrefs.SetInt("EstadoBotonCantar", 1);
        PlayerPrefs.Save();
        int tmp = PlayerPrefs.GetInt("ValorDisponible") + 1;
        PlayerPrefs.SetInt("ValorDisponible", tmp);
        PlayerPrefs.Save();
        TryKillThread();
        SceneManager.LoadScene("RevisionMain");

    }
    public void ButtonGritar()
    {
        sendStringDataSabotaje("Gritar");
        PlayerPrefs.SetInt("EstadoBotonGritar", 1);
        PlayerPrefs.Save();
        int tmp = PlayerPrefs.GetInt("ValorDisponible") + 1;
        PlayerPrefs.SetInt("ValorDisponible", tmp);
        PlayerPrefs.Save();
        TryKillThread();
        SceneManager.LoadScene("RevisionMain");
    }
    public void ButtonLagartija()
    {
        sendStringDataSabotaje("Lagartija");
        PlayerPrefs.SetInt("EstadoBotonLagartija", 1);
        PlayerPrefs.Save();
        int tmp = PlayerPrefs.GetInt("ValorDisponible") + 1;
        PlayerPrefs.SetInt("ValorDisponible", tmp);
        PlayerPrefs.Save();
        TryKillThread();
        SceneManager.LoadScene("RevisionMain");
    }
    public void ButtonSentadilla()
    {
        sendStringDataSabotaje("Sentadilla");
        PlayerPrefs.SetInt("EstadoBotonSentadilla", 1);
        PlayerPrefs.Save();
        int tmp = PlayerPrefs.GetInt("ValorDisponible") + 1;
        PlayerPrefs.SetInt("ValorDisponible", tmp);
        PlayerPrefs.Save();
        TryKillThread();
        SceneManager.LoadScene("RevisionMain");
    }
    public void ButtonTijera()
    {
        sendStringDataSabotaje("Tijera");
        PlayerPrefs.SetInt("EstadoBotonTijera", 1);
        PlayerPrefs.Save();
        int tmp = PlayerPrefs.GetInt("ValorDisponible") + 1;
        PlayerPrefs.SetInt("ValorDisponible", tmp);
        PlayerPrefs.Save();
        TryKillThread();
        SceneManager.LoadScene("RevisionMain");
    }

    public void MasTiempo() 
    {
        sendStringDataCiclista("Tiempo");
    }
    public void MenosTiempo()
    {
        sendStringDataCiclista("NoTiempo");
    }
    public void ButtonRestar()
    {
        BotonRestart.SetActive(true);
        SceneManager.LoadScene("Main");
    }
    IEnumerator Load()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
    }

    void Update()
    {

        if (receiveQueue.Count != 0)
        {
            byte[] message = (byte[])receiveQueue.Dequeue();
            if (message == null)
                return;
            Debug.Log("Mensaje de llegada");
            _dataReceived = Encoding.Default.GetString(message); ;
            Debug.Log(_dataReceived);
            MenosTiempo();
        }



    }
}
