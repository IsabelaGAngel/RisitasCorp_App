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
    private string _ipDataSabotaje = "127.0.0.1";//IP Cell2
    public int Disponible = 3;

    private int _receivePortData = 44444;
    private int _sendPortData = 3100;

    private bool isInitialized;
    private Queue receiveQueue;

    private byte _dataReceived;



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
            
            BotonRestart.SetActive(true);

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
    

    private void sendStringDataCiclista (byte message)
    {
        try
        {
            byte data = message;
            byte[] dataBytes = new byte[1];
            dataBytes[0] = data;
            _dataReceiveCiclista.Send(dataBytes, 1, _receiveEndPointDataCiclista);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }
    private void sendStringDataSabotaje (byte message)
    {
        try
        {
            byte data = message;
            byte[] dataBytes = new byte[1];
            dataBytes[0] = data;
            _dataReceiveSabotaje.Send(dataBytes, 1, _receiveEndPointDataSabotaje);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }

    public void ButtonBailar()
    {
        sendStringDataSabotaje(0x02);
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
        sendStringDataSabotaje(0x03);
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
        sendStringDataSabotaje(0x04);
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
        sendStringDataSabotaje(0x05);
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
        sendStringDataSabotaje(0x06);
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
        sendStringDataSabotaje(0x07);
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
        sendStringDataCiclista(0x0A);
    }
    public void MenosTiempo()
    {
        sendStringDataCiclista(0x0F);
    }
    public void ButtonRestar()
    {
        
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
            _dataReceived = message[0]; ;
            Debug.Log(_dataReceived);
            MenosTiempo();
        }



    }
}
