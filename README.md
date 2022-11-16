![image](https://user-images.githubusercontent.com/109630115/202062672-7b94a457-ee4a-46ed-81ab-5611fb755378.png)
## Additional applications 

Finally, application services, which run different orders of experience and are responsible for detonating different events in the main application. 

These applications run on android devices connected to the same local Wifi network as the other sensors to subsequently send the data and receive them by UDP. 
### Simplified operating steps

1. EXPLANATION OF THE FUNCTIONALITIES
* Master application
* Saboteur application

2. PROTOCOL OF ACTIVITIES
* Sending data
* Verify connections

***Explanation of the functionalities***

![](https://s3.us-west-2.amazonaws.com/secure.notion-static.com/70d5f4ac-513e-4f08-963e-04d497205118/Untitled.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20221115%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20221115T235057Z&X-Amz-Expires=86400&X-Amz-Signature=3ce033860bd6cf8a1a25958a6cb2cde491ae84b8ddcecc69bbdef735211a9eac&X-Amz-SignedHeaders=host&response-content-disposition=filename%3D"Untitled.png"&x-id=GetObject)

External applications are part of the 3 and 4 UDP client; These affect their own actions directly.

**Master application**

The Master's device that will send the tasks to the saboteur and the advantages or disadvantages to the cyclist.Which has 3 types of data, The first is the conservation of names of both the rider and the saboteur:

```c#
public void ButtonContinuar()
    {
        sendStringDataCiclista(NombreCiclista.text);
        sendStringDataSabotaje(NombreSaboteador.text);
        Debug.Log(PlayerPrefs.GetInt("EstadoBotonBailar"));

        TryKillThread();
        SceneManager.LoadScene("ActividadesMaster");
    }
```

 The second is the type of activity that will be sent to the saboteur:

```c#
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
```

Finally the result of the activity carried out that directly affected the cyclist:

```c#
public void MasTiempo() 
    {
        sendStringDataCiclista(0x0A);
    }
public void MenosTiempo()
    {
        sendStringDataCiclista(0x0F);
    }
```
**Saboteur application**

A saboteur device that will give tasks to the user who fulfills this role. Saboteurs mostly receive data, which allows them to know what reaction or activity they should do, receive three types of data; The first is your name:

```c#
if (receiveQueue.Count != 0)
    {
            byte[] message = (byte[])receiveQueue.Dequeue();
        if (message == null)
            return;
        Debug.Log("Mensaje de llegada");
        _dataReceived = Encoding.Default.GetString(message); ;
        Debug.Log(_dataReceived);
        Nombre.text = _dataReceived;
    }
```

The second the activity to be carried out:

```c#
if (receiveQueue.Count != 0)
    {
        byte[] message = (byte[])receiveQueue.Dequeue();
        if (message == null)
         return;
        Debug.Log("Mensaje de llegada");
        dataReceived = message[0];
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
    }
```

The last the verification of the activity:

```c#
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
```


The only data they send are the confirmation of sabotage:

```c#
public void ButtonAccion()
    {
        byte tmpByte = 0x01;
        sendStringData(tmpByte);
        Uso.SetActive(true);
        Accion1.SetActive(false);
                                         
    }
```

***Protocol of activities***

**Sending data**

To send and receive the data from the master, two IPEndPoint are generated whose objective is to be assigned to each of the other users to send the corresponding data.

```c#
    private Thread receiveThread;
    private UdpClient _dataReceiveCiclista;
    private UdpClient _dataReceiveSabotaje;
    private IPEndPoint _receiveEndPointDataCiclista;
    private IPEndPoint _receiveEndPointDataSabotaje;
    public string _ipDataCiclista = "192.168.100.13"; //IP PC
    private string _ipDataSabotaje = "127.0.0.1";//IP SAB
    public int Disponible = 3;

    private int _receivePortData = 44444;
    private int _sendPortData = 3100;
```

As seen in the code each one is assigned its IP address and its serial port to avoid failures.

**Verify connections**

To verify the connections between the applications we will use the scriptCommunicator program; facilitating the process of reviewing and filtering data.

![](https://a.fsdn.com/con/app/proj/scriptcommunicator/screenshots/2017-08-07_16h01_36.png/max/max/1)

*Remember that application deployment code is not written in this repository; To see the full functionality download a version of the following link*

[Risitas Corp. APP Repository](https://github.com/IsabelaGAngel/RisitasCorp_App)


![Logo](https://s3.us-west-2.amazonaws.com/secure.notion-static.com/c677e014-096e-4d5c-9a1a-967260f910ef/Untitled.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20221116%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20221116T003300Z&X-Amz-Expires=86400&X-Amz-Signature=bba3963933c824c412e7fc360d77ca6f422734219919733b2750cf24d77093d8&X-Amz-SignedHeaders=host&response-content-disposition=filename%3D"Untitled.png"&x-id=GetObject)
