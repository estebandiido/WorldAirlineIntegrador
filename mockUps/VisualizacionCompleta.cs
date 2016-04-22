﻿using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Mundo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mockUps
{
    public partial class VisualizacionCompleta : Form
    {

        private List<Arista<Ciudad>> grafoViajero;
        public VisualizacionCompleta()
        {
            InitializeComponent();
        }

        public void asignarSolucion(List<Arista<Ciudad>> solucion)
        {
            grafoViajero = solucion;
        }


        private void VisualizacionCompleta_Load(object sender, EventArgs e)
        {
            mapaSolucion.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            mapaSolucion.DragButton = MouseButtons.Left;
            checkBoxVista.Text = "Ver Destinos" + "\n" +
                                  "con rutas" + "\n" +
                                  "y distancias";
        }

        public void informacionViajero(string i,string n,string ciu,string solucion)
        {
            labId.Text += " "+i;
            labNombre.Text += " " + n;

            foreach (string ciudad in ciu.Split('#'))
            {
                listCiudades.Items.Add(ciudad);
            }
            foreach (string ciudad in solucion.Split('#'))
            {
                listSolucion.Items.Add(ciudad);
            }

        }
        public void dibujarSolucion(bool ponderado)
        {
            mapaSolucion.Overlays.Clear();
            
            GMapOverlay marcadores = new GMapOverlay("ciudades");
            GMapOverlay lineas = new GMapOverlay("rutas");
            if (ponderado)
            {

           

                int i = 0;
                Arista<Ciudad>   primera =grafoViajero.First();
                foreach (Arista<Ciudad> arista in grafoViajero)
                {
                    
                    Ciudad inicio = arista.Destino1;
                    Ciudad fin = arista.Destino2;
                    double distancia = Math.Round(arista.Distancia,2);
                    GMarkerGoogle iniciom = null;
                    GMarkerGoogle finm = null;
                  
                        iniciom = new GMarkerGoogle(new PointLatLng(inicio.Latitud, inicio.Longitud), GMarkerGoogleType.blue_dot);
               
                        finm = new GMarkerGoogle(new PointLatLng(fin.Latitud, fin.Longitud), GMarkerGoogleType.blue_dot);

                    iniciom.ToolTipText = "Nombre: " + inicio.Nombre + "\n" +
                                        "Latitud : " + inicio.Latitud + "\n" +
                                        "Longitud : " + inicio.Longitud + "\n" +
                                        "Poblacion : " + inicio.TotalPoblacion;
                    finm.ToolTipText = "Nombre: " + fin.Nombre + "\n" +
                                        "Latitud : " + fin.Latitud + "\n" +
                                        "Longitud : " + fin.Longitud + "\n" +
                                        "Poblacion : " + fin.TotalPoblacion;
                    marcadores.Markers.Add(iniciom);
                    marcadores.Markers.Add(finm);
                    List<PointLatLng> points = new List<PointLatLng>();
                    points.Add(new PointLatLng(inicio.Latitud, inicio.Longitud));
                    points.Add(new PointLatLng(fin.Latitud, fin.Longitud));
                    GMapPolygon polygon = new GMapPolygon(points, "mypolygon");
                    polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                    polygon.Stroke = new Pen(Color.Red, 5);
                    lineas.Polygons.Add(polygon);
                    double lat = (inicio.Latitud + fin.Latitud) / 2;
                    double lon = (inicio.Longitud + fin.Longitud) / 2;
                    PointLatLng puntoDistancia = new PointLatLng(lat, lon);
                    GMarkerCross marcadorDistancia = new GMarkerCross(puntoDistancia);
                    marcadorDistancia.ToolTipText = distancia + " Km";
                    marcadorDistancia.ToolTipMode = MarkerTooltipMode.Always;

                    marcadores.Markers.Add(marcadorDistancia);
                    mapaSolucion.Overlays.Add(marcadores);
                    mapaSolucion.Overlays.Add(lineas);
                    i++;
                }
        
            }
            else
            {

                int i = 0;
                foreach (Arista<Ciudad> arista in grafoViajero)
                {
                    int cantidadAristas = grafoViajero.Count();
                    Ciudad inicio = arista.Destino1;
                    Ciudad fin = arista.Destino2;
                    Ciudad ciudadInicial = null;
                    if (i==0)
                    {
                        ciudadInicial = inicio;
                    }
                    
                    double distancia = arista.Distancia;
                    GMarkerGoogle iniciom = null;
                    GMarkerGoogle finm = null;
      
                    if (ciudadInicial!=null&&ciudadInicial.Id.Equals(inicio.Id))
                    {
                        iniciom = new GMarkerGoogle(new PointLatLng(inicio.Latitud, inicio.Longitud), GMarkerGoogleType.blue);
                    }
                    else
                    {
                        iniciom = new GMarkerGoogle(new PointLatLng(inicio.Latitud, inicio.Longitud), GMarkerGoogleType.blue_dot);
                    }
                    finm = new GMarkerGoogle(new PointLatLng(fin.Latitud, fin.Longitud), GMarkerGoogleType.blue_dot);

                    iniciom.ToolTipText = "Nombre: " + inicio.Nombre + "\n" +
                                        "Latitud : " + inicio.Latitud + "\n" +
                                        "Longitud : " + inicio.Longitud + "\n" +
                                        "Poblacion : " + inicio.TotalPoblacion;
                    finm.ToolTipText = "Nombre: " + fin.Nombre + "\n" +
                                        "Latitud : " + fin.Latitud + "\n" +
                                        "Longitud : " + fin.Longitud + "\n" +
                                        "Poblacion : " + fin.TotalPoblacion;
                    marcadores.Markers.Add(iniciom);
                    marcadores.Markers.Add(finm);                                      
                    mapaSolucion.Overlays.Add(marcadores);
                    i++;
                }
                //MessageBox.Show("en proceso de desarrollo, gracias por su paciencia");
            }
            mapaSolucion.Zoom = 0;
            mapaSolucion.Zoom = 1;
            mapaSolucion.Zoom = 0;

        }
        private void mapaSolucion_Load(object sender, EventArgs e)
        {

        }

        private void checkBoxVista_CheckedChanged(object sender, EventArgs e)
        {
            mapaSolucion.Overlays.Clear();
            dibujarSolucion(checkBoxVista.Checked);
        }
    }
}
