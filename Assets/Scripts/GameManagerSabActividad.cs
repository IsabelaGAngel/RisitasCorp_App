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
    private string _ipData = "127.0.0.1";

    private int _receivePortData = 3100;
    private int _sendPortData = 44444;

    private bool isInitialized;
    private Queue receiveQueue;

    private byte _dataReceived;

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

    private void sendStringData(byte message)
    {
        try
        {
            byte data = message;
            byte[] dataBytes = new byte[1];
            dataBytes[0] = data;
            _dataReceiveClient.Send(dataBytes, 1, _receiveEndPointData);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }

    public void ButtonAccion()
    {
        byte tmpByte = 0x01;
        sendStringData(tmpByte);
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
            _dataReceived = message[0];
            Debug.Log(_dataReceived);

            if (_dataReceived==0X02){
                CartaBailar.SetActive(true);
            }
            else if (_dataReceived==0X03)
            {
                CartaCantar.SetActive(true);
            }
            else if (_dataReceived==0X04)
            {
                CartaGritar.SetActive(true);
            }
            else if (_dataReceived==0X05)
            {
                CartaLagartija.SetActive(true);
            }
            else if (_dataReceived==0X06)
            {
                CartaSentadilla.SetActive(true);
            }
            else if (_dataReceived==0X07)
            {
                CartaTijera.SetActive(true);
            }
            else if (_dataReceived==0X10)
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
            else if (_dataReceived==0X11)
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
            else if (_dataReceived==0x12)
            {
                Debug.Log("YUjooooooooooooooooo");
            }
            
        }
    }
}
