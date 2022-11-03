using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerSabActividad : MonoBehaviour
{
    [SerializeField] GameObject CartaBailar;
    [SerializeField] GameObject CartaCantar;
    [SerializeField] GameObject CartaGritar;
    [SerializeField] GameObject CartaSentadilla;
    [SerializeField] GameObject CartaLagartija;
    [SerializeField] GameObject CartaTijera;
    [SerializeField] GameObject Paso;
    [SerializeField] GameObject NoPaso;
    [SerializeField] GameObject Accion1;
    [SerializeField] GameObject Accion2;
    [SerializeField] GameObject Uso;
    [SerializeField] GameObject Mensaje;


    private static GameManagerSabActividad instance;
    private Thread receiveThread;
    private UdpClient _dataReceiveClient;
    private IPEndPoint _receiveEndPointData;
    public string _ipData = "127.0.0.1";

    public int _receivePortData = 3100;
    public int _sendPortData = 44444;

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
        _receiveEndPointData = new IPEndPoint(IPAddress.Parse(_ipData), _sendPortData);
        _dataReceiveClient = new UdpClient(_receivePortData);
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
                byte[] dataPulse = _dataReceiveClient.Receive(ref _receiveEndPointData);
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

            _dataReceiveClient.Close();
            _dataReceiveClient = null;

            Debug.Log("Thread killed");
            isInitialized = false;
        }
    }

    private void sendStringData(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _dataReceiveClient.Send(data, data.Length, _receiveEndPointData);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }

    public void ButtonAccion()
    {
        
        sendStringData("1");
        Uso.SetActive(true);
        Accion1.SetActive(false);
                                         
    }

    public void Quitar()
    {
        Uso.SetActive(false);
        Mensaje.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (receiveQueue.Count != 0)
        {
            byte[] message = (byte[])receiveQueue.Dequeue();
            if (message == null)
                return;
            Debug.Log("Mensaje de llegada");
            _dataReceived = Convert.ToInt32(message[0]);
            Debug.Log(_dataReceived.ToString());

            if (_dataReceived==2){
                    CartaBailar.SetActive(true);
                }
                else if (_dataReceived==3)
                {
                    CartaCantar.SetActive(true);
                }
                else if (_dataReceived==4)
                {
                    CartaGritar.SetActive(true);
                }
                else if (_dataReceived==5)
                {
                    CartaLagartija.SetActive(true);
                }
                else if (_dataReceived==6)
                {
                    CartaSentadilla.SetActive(true);
                }
                else if (_dataReceived==7)
                {
                    CartaTijera.SetActive(true);
                }
                else if (_dataReceived==10)
                {
                    CartaBailar.SetActive(false);
                    CartaCantar.SetActive(false);
                    CartaGritar.SetActive(false);
                    CartaLagartija.SetActive(false);
                    CartaSentadilla.SetActive(false);
                    CartaTijera.SetActive(false);
                    Paso.SetActive(true);
                    Accion1.SetActive(true);
                }
                else if (_dataReceived==11)
                {
                    CartaBailar.SetActive(false);
                    CartaCantar.SetActive(false);
                    CartaGritar.SetActive(false);
                    CartaLagartija.SetActive(false);
                    CartaSentadilla.SetActive(false);
                    CartaTijera.SetActive(false);
                    NoPaso.SetActive(true);
                    Accion2.SetActive(true);
                }
            
        }
    }
}
