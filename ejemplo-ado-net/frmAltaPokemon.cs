using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;

namespace ejemplo_ado_net
{
    public partial class frmAltaPokemon : Form
    { 
        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;
        public frmAltaPokemon()
        {
            InitializeComponent();
        }

        public frmAltaPokemon(Pokemon modificar)
        {
            InitializeComponent();
            pokemon = modificar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            PokemonNegocio negocio = new PokemonNegocio();


            try
            {

                if (pokemon == null)
                {
                    pokemon = new Pokemon();
                }
                    pokemon.Numero = int.Parse(txtNumero.Text);
                    pokemon.Nombre = txtNombre.Text;
                    pokemon.Descripcion = txtDescripcion.Text;
                    pokemon.Tipo = (Elemento)cboTipo.SelectedItem;
                    pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;
                    pokemon.UrlImagen = txtUrlImagen.Text;


                if (pokemon.Id == 0) { 
                    negocio.agregar(pokemon);
                    MessageBox.Show("Agregado Exitosamente");
                }
                else
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado Exitosamente");
                }

                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                }
                    Close();
                    


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio negocio = new ElementoNegocio();
            try
            {
                cboTipo.DataSource = negocio.listar();
                cboTipo.ValueMember = "Id";
                cboTipo.DisplayMember = "Descripcion";
                cboDebilidad.DataSource = negocio.listar();
                cboDebilidad.ValueMember = "Id";
                cboDebilidad.DisplayMember = "Descripcion";

                if (pokemon != null)
                {
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    cboTipo.SelectedValue = pokemon.Tipo.Id;
                    cboDebilidad.SelectedValue = pokemon.Debilidad.Id;
                    cargarImagen(pokemon.UrlImagen);


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }



        private void cargarImagen(string urlImagen)
        {
            try
            {

                pbAltaPokemon.Load(urlImagen);

            }
            catch (Exception)
            {

                pbAltaPokemon.Load("https://img.freepik.com/vector-gratis/ilustracion-icono-galeria_53876-27002.jpg?size=626&ext=jpg&ga=GA1.1.1413502914.1719878400&semt=ais_user");
            }
        }

        private void btnLevantarImg_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                //File.Copy(archivo.FileName,ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

            }
        }
    }
}
