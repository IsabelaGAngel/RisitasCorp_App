using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine.UI;

public class GameManagerSabMain : MonoBehaviour
{
    [SerializeField] Text Nombre;
   
    private static GameManagerSabMain instance;
    private Thread receiveThread;
    private UdpClient _dataReceive;
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
        _dataReceive = new UdpClient(_receivePortData);
        
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
                byte[] dataPulse = _dataReceive.Receive(ref _receiveEndPointData);
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

            _dataReceive.Close();
            _dataReceive = null;

            Debug.Log("Thread killed");
            isInitialized = false;
        }
    }

    private void sendStringDataCiclista(byte message)
    {
        try
        {
            byte data = message;
            byte[] dataBytes = new byte[1];
            dataBytes[0] = data;
            _dataReceive.Send(dataBytes, 1, _receiveEndPointData);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }

    public void ButtonIngresar()
    {
        TryKillThread();
        SceneManager.LoadScene("SActividad");
    }
    // Update is called once per frame
    void Update()
    {
        /*if (receiveQueue.Count != 0)
        {
            byte[] message = (byte[])receiveQueue.Dequeue();
            if (message == null)
                return;
            Debug.Log("Mensaje de llegada");
            _dataReceived = Encoding.Default.GetString(message); ;
            Debug.Log(_dataReceived);
            Nombre.text = _dataReceived;
        }*/
    }
}
