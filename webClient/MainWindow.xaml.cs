using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.WebSockets;
using System.Threading;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

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
            string jsonDataForClient = JsonSerializer.Serialize<Person>(Tom);
            ArraySegment<byte> dataForSend = new ArraySegment<byte>(jsonDataForSend);

            webClient.SendAsync(dataForSend, 0, true, token);
            requestToServerList.Items.Add(jsonDataForClient);


            ArraySegment<byte> dataForRecive = new ArraySegment<byte>(jsonDataForSend);
            webClient.ReceiveAsync(dataForRecive, token);

            responceFromServerList.Items.Add(dataForRecive);

                   
            

            
        }
    }
}
