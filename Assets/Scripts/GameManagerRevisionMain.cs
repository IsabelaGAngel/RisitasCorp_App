using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;

public class GameManagerRevisionMain : MonoBehaviour
{
    [SerializeField] InputField NombreCiclista;
    [SerializeField] InputField NombreSaboteador;

    private static GameManagerRevisionMain instance;
    //private Thread receiveThread;
    private UdpClient _dataReceiveCiclista;
    private UdpClient _dataReceiveSabotaje;
    private IPEndPoint _receiveEndPointDataCiclista;
    private IPEndPoint _receiveEndPointDataSabotaje;
    public string _ipDataCiclista = "192.168.100.13"; //IP PC
    public string _ipDataSabotaje = "192.168.100.5"; //IP Cell2

    public int _receivePortData = 3100;
    public int _sendPortData = 3500;
    private bool isInitialized;
    private Queue receiveQueue;

    private int _dataReceived;



    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        instance = this;
        _receiveEndPointDataCiclista = new IPEndPoint(IPAddress.Parse(_ipDataCiclista), _sendPortData);
        _receiveEndPointDataSabotaje = new IPEndPoint(IPAddress.Parse(_ipDataSabotaje), _sendPortData);
        _dataReceiveSabotaje = new UdpClient(_receivePortData);
        /*receiveQueue = Queue.Synchronized(new Queue());


         receiveThread = new Thread(new ThreadStart(ReceiveDataListener));
         receiveThread.IsBackground = true;
         receiveThread.Start();*/
        isInitialized = true;
    }

    /*    private void ReceiveDataListener()
    {
        while (true)
        {
            try
            {
                byte[] dataPulse = _dataReceiveClient.Receive(ref _receiveEndPointData);
                receiveQueue.Enqueue(dataPulse);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }*/
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
            /*receiveThread.Abort();
            receiveThread = null;*/

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

    public void ButtonSi()
    {
        sendStringDataSabotaje("Si");
        TryKillThread();
        SceneManager.LoadScene("ActividadesMaster");
        
    }
    public void ButtonNo()
    {
        sendStringDataCiclista("Tiempo");
        sendStringDataSabotaje("No");
        TryKillThread();
        SceneManager.LoadScene("ActividadesMaster");
        
    }

    // Update is called once per frame
    void Update()
    {


    }
}
