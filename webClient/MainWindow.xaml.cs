using System;
using System.Windows;
using System.Net.WebSockets;
using System.Threading;
using System.Text.Json;
using System.Text;

namespace webClient
{
    class Person
    {
        public string name { get; set; }
        public int age { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ClientWebSocket webClient;
        Uri uri = new Uri("ws://localhost:8765");
        CancellationToken token = new CancellationToken();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            webClient = new ClientWebSocket();
            webClient.ConnectAsync(uri,token);
        }
        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            Person Tom = new Person { name = "Tom", age = 35 };
            byte[] jsonDataForSend = JsonSerializer.SerializeToUtf8Bytes<Person>(Tom);
            ArraySegment<byte> dataForSend = new ArraySegment<byte>(jsonDataForSend);
            ArraySegment<byte> bufferForRecv = new ArraySegment<byte>(new byte[1024]);


            webClient.SendAsync(dataForSend, 0, true, token);
            requestToServerList.Items.Add(Encoding.UTF8.GetString(dataForSend.Array));

            webClient.ReceiveAsync(bufferForRecv, token);
            responceFromServerList.Items.Add(Encoding.UTF8.GetString(bufferForRecv.Array));

        }
    }
}
