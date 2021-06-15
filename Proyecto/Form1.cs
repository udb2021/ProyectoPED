using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;


namespace Proyecto
{
    public partial class Form1 : Form
    {
        //La clase Cola es una implementacion propia del TAD "Cola"
        Cola<Usuario> Usuarios = new Cola<Usuario>();
        //La clase Lista es una implementacion propia del TAD "List"
        Lista<Pedido> Pedidos = new Lista<Pedido>();

        public Form1()
        {
            InitializeComponent();
        }

        public SqlConnection crearConexion()
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Proyecto.Properties.Settings.PupuseriaPrograConnectionString"].ConnectionString);
            return new SqlConnection(builder.ConnectionString);

        }
        private void btnIniciarPedido_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bienvenido!!");
            tabControl1.SelectedIndex = 1;
        }

        public bool ValidarDui(string Dui)
        {
            bool valor = false;
            Regex regex = new Regex("[0-9]+[0-9]+[0-9]+[0-9]+[0-9]+[0-9]+[0-9]+[0-9]+-+[0-9]$");
            Match match = regex.Match(txtDUI.Text);

            if (match.Success)
            {
                valor = true;
            }

            return valor;
        }

        private void btnUsuario_Click(object sender, EventArgs e)
        {

            string DUI = txtDUI.Text;
            //Validar Dui
            bool comprobar = ValidarDui(DUI);

            if (comprobar == true)
            {
                try
                {
                    //Establecemos conexión con el servidor
                    //SqlConnection Con = new SqlConnection("Data Source=DELL17" + "\\" + "SQLEXPRESS;Initial Catalog=PupuseriaProgra;Integrated Security=True");
                    SqlConnection Con = crearConexion();
                    Con.Open();

                    //Recuperamos datos del usuario
                    string CodigoUsuario = txtDUI.Text;
                    string Nombre = txtNombre.Text;
                    string fecha = Convert.ToString(DateTime.Now);

                    //Creamos datos de Ticket

                    string ticket =  CodigoUsuario + "-"+"PP2020";

                    //Inserción en la base de datos
                    string cadena = "insert into Usuario(CodigoUsuario,NombreUsuario,FechaPedido) values ('"+CodigoUsuario+"','"+Nombre+"','"+fecha+"')";
                    string cadena2 = "insert into Ticket(CodigoTicket,CodigoUsuario,Fecha) values ('"+ticket+"','"+CodigoUsuario+ "','" + fecha + "')";
                    SqlCommand Comando = new SqlCommand(cadena, Con);
                    SqlCommand Comando2 = new SqlCommand(cadena2, Con);
                    Comando.ExecuteNonQuery();
                    Comando2.ExecuteNonQuery();

                    //Se confirma el registro y se mueve a la siguiente ventanilla
                    MessageBox.Show("Usuario registrado con éxito");
                    tabControl1.SelectedIndex = 2;

                    //Cerramos Conexión
                    Con.Close();

                }
                catch
                {
                    MessageBox.Show("Error de conexión");
                }
            }
            else
            {
                MessageBox.Show("Compruebe sus datos");
            }


        }
 
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            string Harina = "";
            double FrijolQueso;
            double Revueltas;
            double Chicharron;
            double Queso;
            string CodigoHarina = "";

            try
            {
                //Selección de Harina
                if (ChMaiz.Checked == true)
                {
                    Harina = "Maiz";
                    CodigoHarina = "0001";
                   
                }
                else if(ChArroz.Checked == true)
                {
                    Harina = "Arroz";
                    CodigoHarina = "0002";

                }
                else
                {
                    MessageBox.Show("No ha seleccionado su harina");
                }

                //Calculo de pupusas seleccionadas
                if(FQ.Checked == true)
                {
                    FrijolQueso = 0.60 * Convert.ToDouble(CantidadFQ.Value);
                    LbFQ.Text = "$"+Convert.ToString(FrijolQueso);
                }
                else
                {
                    FrijolQueso = 0;
                }

                if (Rev.Checked == true)
                {
                    Revueltas = 0.60 * Convert.ToDouble(CantidadRev.Value);
                    LbRev.Text = "$" + Convert.ToString(Revueltas);

                }
                else
                {
                    Revueltas = 0;
                }

                if (Chi.Checked == true)
                {
                    Chicharron = 0.60 * Convert.ToDouble(CantidadChi.Value);
                    LbChi.Text = "$" + Convert.ToString(Chicharron);
                 }
                else
                {
                    Chicharron = 0;
                }

                if (Que.Checked == true)
                {
                    Queso = 0.60 * Convert.ToDouble(CantidadQue.Value);
                    LbQue.Text = "$" + Convert.ToString(Queso);

                }
                else
                {
                    Queso = 0;
                }

                //Calculando total
                double total = FrijolQueso + Revueltas + Chicharron + Queso;
                LbTotal.Text = "$" + Convert.ToString(total);
                string totalDB = total.ToString();

                string CodigoUsuario = string.Empty;
                string CodigoTicket = string.Empty;
                string UsuarioID = string.Empty;
                string CodigoPedido = string.Empty;
                string pedido = Harina + "FQ" + CantidadFQ.Value.ToString() + "R" + CantidadRev.Value.ToString() + "Chi" + CantidadChi.Value.ToString() + "Q" + CantidadQue.Value.ToString();

                //Establecemos conexión con el servidor
                //SqlConnection Con = new SqlConnection("Data Source=DELL17" + "\\" + "SQLEXPRESS;Initial Catalog=PupuseriaProgra;Integrated Security=True");
                SqlConnection Con = crearConexion();

                Con.Open();
                    string consulta = "SELECT Top 1 Usuario.CodigoUsuario, CodigoTicket, Usuario.ID FROM Usuario INNER JOIN Ticket ON Usuario.CodigoUsuario = Ticket.CodigoUsuario Order By Usuario.ID Desc";
                    SqlCommand Comando3 = new SqlCommand(consulta, Con);
                    SqlDataReader lector = Comando3.ExecuteReader();
                
                    try {
                    //Verifica que se puedan leer los registros
                    if (lector.HasRows)
                        {
                            //Agrega datos a variables creadas anteriormente
                            while (lector.Read())
                            {
                                CodigoUsuario = lector["CodigoUsuario"].ToString();
                                CodigoTicket = lector["CodigoTicket"].ToString();
                                UsuarioID = lector["ID"].ToString();
                                CodigoPedido = UsuarioID +"-"+ CodigoUsuario;

                            //SqlConnection Con2 = new SqlConnection("Data Source=DELL17" + "\\" + "SQLEXPRESS;Initial Catalog=PupuseriaProgra;Integrated Security=True");
                            SqlConnection Con2 = crearConexion();

                            Con2.Open();
                            //Inserción de pedido
                            string cad = "insert into Pedidos(CodigoPedidos,CodigoUsuario,CodigoTicket,Pedido) values ('" + CodigoPedido + "','" + CodigoUsuario + "','" + CodigoTicket + "','" + pedido + "')";
                            string cad2 = "insert into Combos(CodigoCombo,NombreCombo,CodigoPedidos,Precio) values ('"+CodigoPedido+"','Individual','" + CodigoPedido + "','" + totalDB + "')";

                            SqlCommand Com = new SqlCommand(cad, Con2);
                            SqlCommand Com2 = new SqlCommand(cad2, Con2);

                            Com.ExecuteNonQuery();
                            Com2.ExecuteNonQuery();
                            
                        }
                    }

                    MessageBox.Show("Pedido creado con éxito");
                    }
                catch{  MessageBox.Show("Error de inserción"); }
            }
            catch
            {
                MessageBox.Show("Error"); 
            }
        }

        private void btnContinuarPedido_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void btnCombo_Click(object sender, EventArgs e)
        {
            double total = 0;
            double Combo1_Precio = 0;
            double Combo2_Precio = 0;
            double Combo1F_Precio = 0;
            double Combo2F_Precio = 0;
            string CodigoComboTotal = "";
            string CodigoCombo1 = "";
            string CodigoCombo2 = "";
            string CodigoCombo3 = "";
            string CodigoCombo4 = "";
            string CodigoHarina = "";
            string Harina = "";
                //Selección de Harina
                if (ComboMaiz.Checked == true)
                {
                    Harina = "Maiz";
                    CodigoHarina = "0001";

                }
                else if (ComboArroz.Checked == true)
                {
                    Harina = "Arroz";
                    CodigoHarina = "0002";

                }

                if (Combo1.Checked == true)
            {
                Combo1_Precio = 2.50;
                CodigoCombo1 = "In1";
            }
            else
            {
                Combo1_Precio = 0;
                CodigoCombo1 = "";
                }

            if (Combo2.Checked == true)
            {
                Combo2_Precio = 3.00;
                CodigoCombo2 = "In2";

            }
            else
            {
                Combo2_Precio = 0;
                CodigoCombo2 = "";

                }

            if (Combo1F.Checked == true)
            {
                Combo1F_Precio = 12.50;
                CodigoCombo3 = "Fam1";

            }
            else
            {
                Combo1F_Precio = 0;
                CodigoCombo3 = "";

                }
            if (Combo2F.Checked == true)
            {
                Combo2F_Precio = 15.00;
                CodigoCombo4 = "Fam2";

            }
            else
            {
                Combo2F_Precio = 0;
                CodigoCombo4 = "";

                }

            total = Combo1_Precio + Combo2F_Precio + Combo2_Precio + Combo1F_Precio;
            CodigoComboTotal = Harina +"-"+ CodigoCombo1 +"-"+ CodigoCombo2 +"-"+ CodigoCombo3 +"-"+ CodigoCombo4;
            
            string CodigoUsuario = string.Empty;
            string UsuarioID = string.Empty;
            string CodigoPedido = string.Empty;
            string CodigoTicket = string.Empty;
            string totalDB = Convert.ToString(total);
            string CodigoDB = Convert.ToString(CodigoComboTotal);

            //Establecemos conexión con el servidor
            //SqlConnection Con1 = new SqlConnection("Data Source=DELL17" + "\\" + "SQLEXPRESS;Initial Catalog=PupuseriaProgra;Integrated Security=True");
            SqlConnection Con1 = crearConexion();

            Con1.Open();
            string consulta = "SELECT Top 1 Usuario.CodigoUsuario, Pedidos.CodigoPedidos, CodigoTicket, Usuario.ID FROM Usuario INNER JOIN Pedidos ON Usuario.CodigoUsuario = Pedidos.CodigoUsuario Order By Usuario.ID Desc";
            SqlCommand Comando3 = new SqlCommand(consulta, Con1);
            SqlDataReader lector = Comando3.ExecuteReader();

            try
            {
                //Verifica que se puedan leer los registros
                if (lector.HasRows)
                {
                    //Agrega datos a variables creadas anteriormente
                    while (lector.Read())
                    {
                        CodigoUsuario = lector["CodigoUsuario"].ToString();
                        CodigoTicket = lector["CodigoTicket"].ToString();
                        UsuarioID = lector["ID"].ToString();
                        CodigoPedido = lector["CodigoPedidos"].ToString();
                        string combocode = UsuarioID + "- Grupal";
                        string CodigoDBTotal = string.Empty;
                        if (NoCombo.Checked == true)
                        {
                            CodigoDBTotal = "No/Combo || " + UsuarioID;
                        }
                        else
                        {
                             CodigoDBTotal = CodigoDB + " || " + UsuarioID;
                        }

                        //SqlConnection Con2 = new SqlConnection("Data Source=DELL17" + "\\" + "SQLEXPRESS;Initial Catalog=PupuseriaProgra;Integrated Security=True");
                        SqlConnection Con2 = crearConexion();
                        Con2.Open();
                        //Inserción de pedido
                        string cad2 = "insert into Combos(CodigoCombo,NombreCombo,CodigoPedidos,Precio) values ('"+ combocode +"','"+ CodigoDBTotal +"','" + CodigoPedido + "','" + totalDB + "')";

                        SqlCommand Com = new SqlCommand(cad2, Con2);

                        Com.ExecuteNonQuery();
                        Con2.Close();    
                    }
                    
                }

                MessageBox.Show("Orden aceptada");
                Con1.Close();

            }
            catch { MessageBox.Show("Compruebe sus datos si ha escogido un combo, de lo contrario verifiqué que seleccionó no comprar combos"); }

        }

        private void btnContinuarCombo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void btnContinuarBebida_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void btnBebida_Click(object sender, EventArgs e)
        {
            double CocaCola = 0;
            double Pepsi = 0;
            double Fanta = 0;
            double Uva = 0;
            
            //Calculo de bebida seleccionadas
            if (CbCoca.Checked == true)
            {
                CocaCola = 0.60 * Convert.ToDouble(C_Coca.Value);
                Lb_Coca.Text = "$" + Convert.ToString(CocaCola);
            }
            else
            {
                CocaCola = 0;
            }

            if (CbPepsi.Checked == true)
            {
                Pepsi = 0.85 * Convert.ToDouble(C_Pepsi.Value);
                Lb_Pepsi.Text = "$" + Convert.ToString(Pepsi);
            }
            else
            {
                Pepsi = 0;
            }

            if (CbFanta.Checked == true)
            {
                Fanta = 0.60 * Convert.ToDouble(C_Fanta.Value);
                Lb_Fanta.Text = "$" + Convert.ToString(Fanta);
            }
            else
            {
                Fanta = 0;
            }

            if (CbUva.Checked == true)
            {
                Uva = 0.60 * Convert.ToDouble(C_Uva.Value);
                Lb_Uva.Text = "$" + Convert.ToString(Uva);
            }
            else
            {
                Uva = 0;
            }

            //Calculando total
            double total = CocaCola + Pepsi + Fanta + Uva;
            Lb_Total.Text = "$" + Convert.ToString(total);
            string totalDB = total.ToString();


            string CodigoBebida = string.Empty;
            string CodigoPedido = string.Empty;
            string CodigoTicket = string.Empty;


            string pedido = "Coca" + C_Coca.Value.ToString() + "- Fanta" + C_Fanta.Value.ToString() + "- Pepsi" + C_Pepsi.Value.ToString() + "- Uva" + C_Uva.Value.ToString();

            //Establecemos conexión con el servidor
            //SqlConnection Conexion = new SqlConnection("Data Source=DELL17" + "\\" + "SQLEXPRESS;Initial Catalog=PupuseriaProgra;Integrated Security=True");
            SqlConnection Conexion = crearConexion();

            Conexion.Open();
            string consulta = "SELECT Top 1 Usuario.CodigoUsuario, Pedidos.CodigoPedidos, CodigoTicket, Usuario.ID FROM Usuario INNER JOIN Pedidos ON Usuario.CodigoUsuario = Pedidos.CodigoUsuario Order By Usuario.ID Desc";
            SqlCommand ComandoBebida = new SqlCommand(consulta, Conexion);

            SqlDataReader lector = ComandoBebida.ExecuteReader();

            try
            {
                //Verifica que se puedan leer los registros
                if (lector.HasRows)
                {
                    //Agrega datos a variables creadas anteriormente
                    while (lector.Read())
                    {
                        CodigoPedido = lector["CodigoPedidos"].ToString();
                        CodigoTicket = lector["CodigoTicket"].ToString();
                        CodigoBebida = "B - " + CodigoTicket;
                        //SqlConnection Con2 = new SqlConnection("Data Source=DELL17" + "\\" + "SQLEXPRESS;Initial Catalog=PupuseriaProgra;Integrated Security=True");
                        SqlConnection Con2 = crearConexion();

                        Con2.Open();
                        //Inserción de pedido
                        string cad2 = "insert into Bebidas(CodigoBebida,NombreBebida,Precio,CodigoPedidoFK) values ('"+CodigoBebida+"','"+pedido+"','" + totalDB + "','" + CodigoPedido + "')";
                        
                        SqlCommand Com2 = new SqlCommand(cad2, Con2);
                        Com2.ExecuteNonQuery();
                        Con2.Close();

                    }
                }
                else { MessageBox.Show("No se pudo recuperar el código generado"); }

                MessageBox.Show("Orden de bebidas completada");
            }
            catch { MessageBox.Show("Error de inserción"); }

        }

        //Funcion que obtiene los tamaños de las columnas del datagridview
        public float[]GetTamañoColumnas(DataGridView dg)
        {
            //Tomamos el número de columnas
            float[] values = new float[dg.ColumnCount];
            for(int i = 0; i < dg.ColumnCount; i++)
            {
                //Tomamos el ancho de cadda columna
                values[i] = (float)dg.Columns[i].Width;
            }
            return values;
        }

        public void GenerarDocumentos(Document document)
        {
            //Creamos unn objeto PDFTable con el número de columnas del DGV
            PdfPTable datatable = new PdfPTable(DGVTicket.ColumnCount);

            //Asignamos propiedades para el diseño del pdf
            datatable.DefaultCell.Padding = 1;
            float[] headerwidths = GetTamañoColumnas(DGVTicket);

            datatable.SetWidths(headerwidths);
            datatable.WidthPercentage = 100;
            datatable.DefaultCell.BorderWidth = 1;

            //Definimos el color de las celdas en el pdf
            datatable.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;

            //Color de los bordes
            datatable.DefaultCell.BorderColor = iTextSharp.text.BaseColor.BLACK;

            //Fuente
            iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA);

            Phrase objP = new Phrase("A", fuente);

            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            //Se genera el encabezado de la tabla en el PDF
            for(int i = 0; i< DGVTicket.ColumnCount; i++)
            {
                objP = new Phrase(DGVTicket.Columns[i].HeaderText, fuente);
                datatable.HorizontalAlignment = Element.ALIGN_CENTER;

                datatable.AddCell(objP);
            }

            datatable.HeaderRows = 1;

            datatable.DefaultCell.BorderWidth = 1;

            //Se genera el cuerpo del PDF
            for(int i = 0; i < DGVTicket.RowCount; i++)
            {
                for(int j = 0; j < DGVTicket.ColumnCount; j++)
                {
                    objP = new Phrase(DGVTicket[j, i].Value.ToString(), fuente);
                    datatable.AddCell(objP);
                }
                datatable.CompleteRow();
            }
            document.Add(datatable);
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            if(DGVTicket.RowCount == 0)
            {
                MessageBox.Show("No hay datos para generar Reporte de ventas");
            }
            else
            {
                //Escogeremos ruta donde guardar el PDF
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                if(save.ShowDialog() == DialogResult.OK)
                {
                    string filename = save.FileName;
                    Document doc = new Document(PageSize.A3, 9, 9, 9, 9);
                    Chunk encab = new Chunk("-----------------------------PUPUSERÍA LOS TERCIOS-------------------------\n" +
                                            "----------------------------SIERRA MORENA SOYAPANGO-----------------------\n" +
                                            "                                            \n" +
                                            "HORARIOS DE ATENCIÓN : MARTES A DOMINGO DE 8:00 A 10:30 AM Y  DE 4:00 A 10:00 PM\n" +
                                            "----------TELÉFONOS:                                                        \n" +
                                            "----------LOCAL: 2503-6567            WHATSAPP: 7766-5544                     \n", FontFactory.GetFont("Arial", 16));
                    try
                    {
                        FileStream file = new FileStream(filename, FileMode.OpenOrCreate);
                        PdfWriter writer = PdfWriter.GetInstance(doc, file);
                        writer.ViewerPreferences = PdfWriter.PageModeUseThumbs;
                        writer.ViewerPreferences = PdfWriter.PageLayoutOneColumn;
                        doc.Open();

                        doc.Add(new Paragraph(encab));
                        GenerarDocumentos(doc);

                        Process.Start(filename);
                        doc.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }

        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            //Establecemos conexión con el servidor
            //SqlConnection ConDGV = new SqlConnection("Data Source=DELL17" + "\\" + "SQLEXPRESS;Initial Catalog=PupuseriaProgra;Integrated Security=True");
            SqlConnection ConDGV = crearConexion();
            ConDGV.Open();

            string consulta = "SELECT top 1 Usuario.ID, Usuario.CodigoUsuario, NombreUsuario, Pedidos.CodigoPedidos, Pedidos.CodigoTicket FROM Usuario INNER JOIN Pedidos ON Usuario.CodigoUsuario = Pedidos.CodigoUsuario ORDER BY ID desc";
            SqlCommand Comandodgv = new SqlCommand(consulta, ConDGV);
            SqlDataReader lector = Comandodgv.ExecuteReader();

            string NombreUsuario = string.Empty;
            string CodUsuario = string.Empty;
            string CodigoPedido = string.Empty;
            string CodTicket = string.Empty;

            //Verifica que se puedan leer los registros
            try
            {
                if (lector.HasRows)
                {
                    //Agrega datos a variables creadas anteriormente
                    while (lector.Read())
                    {
                        NombreUsuario = lector["NombreUsuario"].ToString();
                        CodUsuario = lector["CodigoUsuario"].ToString();
                        CodigoPedido = lector["CodigoPedidos"].ToString();
                        CodTicket = lector["CodigoTicket"].ToString();

                        //Llenando DataGridView de Ticket
                        //Crearemos instancia del objeto usuario
                        Usuario usuario = new Usuario();

                        usuario.NombreUsuario = NombreUsuario;
                        usuario.CodigoPedido = CodigoPedido;
                        usuario.CodigoTicket = CodTicket;
                        usuario.CodigoUsuario = CodUsuario;

                        //Enconlamos los datos de la tabla en la estructura
                        Usuarios.Agregar(usuario);

                        //Ahora mostraremos los datos en el DGV iniciandolo en 0
                        //DGVTicket.AutoGenerateColumns = true;
                        DGVTicket.DataSource = null;
                        DGVTicket.DataSource = Usuarios.ToArray();
                    }
                }
            }
            catch
            {
                MessageBox.Show("No se pudo recuperar datos");
            }
        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            string CodigoPedido = txtCodigoPedido.Text;
            string NombreUsuario = string.Empty;
            string Fecha = string.Empty;
            string Pedido = string.Empty;
            string PedidoInd = string.Empty;
            string PedidoComb = string.Empty;
            string Bebida = string.Empty;
            string PrecioBebida = "0";
            double PrecioInd = 0;
            double precioCom = 0;
            double PrecioPedido = 0;


            //Establecemos conexión con el servidor
            //SqlConnection Conexion = new SqlConnection("Data Source=DELL17" + "\\" + "SQLEXPRESS;Initial Catalog=PupuseriaProgra;Integrated Security=True");
            SqlConnection Conexion = crearConexion();
            Conexion.Open();

            
            string consulta1 = "SELECT Usuario.NombreUsuario,Usuario.FechaPedido,Pedidos.Pedido FROM Usuario INNER JOIN Pedidos ON Usuario.CodigoUsuario = Pedidos.CodigoUsuario WHERE Pedidos.CodigoPedidos like '"+CodigoPedido+"'";


            SqlCommand Comando1 = new SqlCommand(consulta1, Conexion);

            SqlDataReader lector1 = Comando1.ExecuteReader();

            //Verifica que se puedan leer los registros
            try
            {
                if (lector1.HasRows)
                {
                    //Agrega datos a variables creadas anteriormente
                    while (lector1.Read())
                    {
                        NombreUsuario = lector1["NombreUsuario"].ToString();
                        Fecha =         lector1["FechaPedido"].ToString();
                        PedidoInd =     lector1["Pedido"].ToString();
                        lector1.Close();

                        string consulta2 = "Select Bebidas.NombreBebida, Bebidas.Precio as Precio_Bebida from Bebidas INNER JOIN Pedidos ON Bebidas.CodigoPedidoFK = Pedidos.CodigoPedidos WHERE Pedidos.CodigoPedidos like '" + CodigoPedido + "'";
                        SqlCommand Comando2 = new SqlCommand(consulta2, Conexion);
                        SqlDataReader lector2 = Comando2.ExecuteReader();

                        //Segundo lector
                        if (lector2.HasRows) {
                            //Agrega datos a variables creadas anteriormente
                            while (lector2.Read())
                            {
                                Bebida = lector2["NombreBebida"].ToString();
                                PrecioBebida = lector2["Precio_Bebida"].ToString();
                                lector2.Close();

                                string consulta3 = "Select Combos.Precio as Precio_Individual from Combos INNER JOIN  Pedidos ON Combos.CodigoPedidos = Pedidos.CodigoPedidos WHERE Pedidos.CodigoPedidos like '" + CodigoPedido + "' and Combos.NombreCombo like 'Individual'";
                                SqlCommand Comando3 = new SqlCommand(consulta3, Conexion);
                                SqlDataReader lector3 = Comando3.ExecuteReader();

                                //Tercer lector
                                if (lector3.HasRows)
                                {
                                    while (lector3.Read())
                                    {
                                        PrecioInd = Convert.ToDouble(lector3["Precio_Individual"].ToString());
                                        lector3.Close();

                                        string consulta4 = "Select Combos.Precio as Precio_Combo, Combos.NombreCombo from Combos INNER JOIN  Pedidos ON Combos.CodigoPedidos = Pedidos.CodigoPedidos WHERE Pedidos.CodigoPedidos like '" + CodigoPedido + "' and Combos.NombreCombo not like 'Individual'";
                                        SqlCommand Comando4 = new SqlCommand(consulta4, Conexion);
                                        SqlDataReader lector4 = Comando4.ExecuteReader();

                                        //Cuarto y último lector
                                        if (lector4.HasRows)
                                        {
                                            while (lector4.Read())
                                            {
                                                if (lector4["NombreCombo"].ToString() != null)
                                                {
                                                    PedidoComb = lector4["NombreCombo"].ToString();
                                                    precioCom = Convert.ToDouble(lector4["Precio_Combo"]);

                                                    //Llenando DataGridView de Ticket
                                                    //Crearemos instancia del objeto usuario
                                                    Pedido pedido = new Pedido();

                                                    pedido.NombreUsuario = NombreUsuario;
                                                    pedido.FechaPedido = Fecha;
                                                    pedido.Pedido_Seleccionado = PedidoInd + "-" + PedidoComb;
                                                    pedido.Precio_Pedido = Convert.ToString(PrecioInd + precioCom);
                                                    pedido.Bebida = Bebida;
                                                    pedido.Precio_Bebida = PrecioBebida;
                                                    pedido.Total = Math.Round(PrecioInd + precioCom + Convert.ToDouble(PrecioBebida),2);

                                                    //Enconlamos los datos de la tabla en la estructura

                                                    Pedidos.InsertarAlFrente(pedido);

                                                    //Ahora mostraremos los datos en el DGV iniciandolo en 0
                                                    DGVPedido.DataSource = null;
                                                    DGVPedido.DataSource = Pedidos.ToArray();
                                                    lector4.Close();
                                                }
                                                else { MessageBox.Show("El combo no esta definido"); }
                                            }
                                        }

                                    }
                                }
                                else { MessageBox.Show("El pedido no lleva combos"); }
                            }

                        }
                        else { MessageBox.Show("El pedido no lleva bebidas"); }
                    }
                }
                else { MessageBox.Show("No existe el pedido"); }
            }
            catch
            {
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    class Usuario
    {
        //Datos de Usuario
        public string CodigoUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string CodigoPedido { get; set; }


        //Datos de Ticket
        public string CodigoTicket{ get; set; }

    }

    class Pedido
    {
        //Pedido
        public string NombreUsuario { get; set; }
        public string FechaPedido { get; set; }
        public string Pedido_Seleccionado { get; set; }
        public string Precio_Pedido { get; set; }
        public string Bebida { get; set; }
        public string Precio_Bebida { get; set; }
        public double Total { get; set; }


    }
}

