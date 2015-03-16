using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.Odbc;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Collections;
using System.Web;
using System.Collections.Specialized;
using System.Net.Sockets;
using System.Net;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Security;
using System.Collections.Concurrent;
using Thrift.Transport;
using Thrift.Protocol;
using System.Windows.Forms.DataVisualization.Charting;
using Bio.IO.Newick;
namespace WindowsFormsApplication1
{
    
 

    public partial class Form1 : Form
    {
        public static DataGridView alignmentResult = new DataGridView();
        System.Data.Odbc.OdbcConnection OdbcCon; List<string> seq;
        Chart barChart;
        public byte[] photo; String bsynid = "BSYN1", domain = "A", bb = "Ala", sequence = ""; Label l = new Label();
        private static String[] description;
        private static String[] sequence1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var line = ""; seq = new List<string>();
            foreach (string l in richTextBox1.Lines)
            {

                if (string.IsNullOrEmpty(l))
                    break;
                if (l.StartsWith(">"))
                {
                    if (line.Trim().Length > 0)
                    {
                        seq.Add(line);
                        line = "";
                    }
                }
                else
                {
                    if (line.Trim().Length <= 0)
                    {
                        line = l;
                    }
                    else
                    {
                        line += l;
                    }

                }
            }
            if (line.Trim().Length > 0)
            {
                seq.Add(line);
                line = "";
            }
            //testdatabase
       //  string ConStr1 = "DRIVER={MySQL ODBC 5.3 UNICODE Driver};SERVER=mxbase.pharma.uni-sb.de;PORT=3306;DATABASE=Myxobase_Test;UID=srdu01_UT_z34GHq;PWD=srdumx;OPTION=3;";

            //myxobase
           string ConStr1 = "DRIVER={MySQL ODBC 5.3 UNICODE Driver};SERVER=mxbase.mins.uni-saarland.de;PORT=3306;DATABASE=myxobase;UID=srdu01_UT_z34GHq;PWD=srdumx;OPTION=3;";
            OdbcCon = new System.Data.Odbc.OdbcConnection(ConStr1);
            string v1 = "";
            try
            {
                if (OdbcCon.State == ConnectionState.Closed)
                {
                    OdbcCon.Open();

                }
                else
                {
                    
                }
            }
            catch (Exception ds)
            {
                MessageBox.Show(ds.ToString());
            }
            try
            {
                DataSet qz1 = new DataSet();
                OdbcDataAdapter qadater1 = new OdbcDataAdapter("select b_alignment from B_GeneComparisons where b_query_BSRCID = 'BSYN497'", OdbcCon);
                qadater1.Fill(qz1);
                foreach (DataRow row1 in qz1.Tables[0].Rows)
                {
                    MessageBox.Show("ok");
                    for (int i = 0; i < qz1.Tables[0].Columns.Count; i++)
                    {
                        MessageBox.Show(row1[i].ToString());
                        richTextBox1.Text = row1[i].ToString();
                    }

                }
            }
            catch (Exception ds)
            {
                MessageBox.Show(ds.ToString());
            }
        }
        public void xmlimport(string path4file)
        {
            Dictionary<int, string> seqdictionary = new Dictionary<int, string>();
            string path = "", filename = "", extension = "";
            string header = "", class_xml = "", context = "", domain = "", building = "", block = "", prediction = "", interaction = "", genelist = "", modulelist = "";
            string keys4 = "", key_name4 = "", BSRCID = "";
            XmlDocument doc = new XmlDocument();
            Dictionary<string, string> mods = new Dictionary<string, string>();

            XDocument doc1 = XDocument.Load(path4file);

            doc.Load(path4file);
            int strain = 0;



            XmlNode headerNode = doc.SelectSingleNode("/root/Header");
            textBox1.Text = path4file;
            filename = Path.GetFileNameWithoutExtension(textBox1.Text);
            extension = Path.GetExtension(textBox1.Text);
            photo = File.ReadAllBytes(textBox1.Text);
            FileInfo fi = new FileInfo(textBox1.Text);
            long size = fi.Length;
            path = textBox1.Text.Replace(@"\", @"\\");
            header = headerNode.OuterXml;
            try
            {
                OdbcCommand Odbcq98 = new OdbcCommand();
                Odbcq98.CommandText = "SELECT MAX(B_fileID) FROM  B_BiosynthesisFiles";
                Odbcq98.Connection = OdbcCon;
                DataSet e111 = new DataSet();
                OdbcDataAdapter Odbce111 = new OdbcDataAdapter(Odbcq98);
                Odbce111.Fill(e111, "B_BiosynthesisFiles");
                foreach (DataRow row1 in e111.Tables[0].Rows)
                {

                    for (int i1 = 0; i1 < e111.Tables[0].Columns.Count; i1++)
                    {
                        keys4 = row1[i1].ToString();
                    }
                }
            }
            catch (Exception fds)
            {
                MessageBox.Show(fds.ToString());
            }
            if ((keys4 == "") || (keys4 == "0"))
            {
                keys4 = "1";
            }
            else
            {

                int w = int.Parse(keys4);
                keys4 = (w + 1).ToString();
            }

            key_name4 = "BXML" + keys4;

            //  MessageBox.Show(key_name4);
            string confidence1 = "m";
            string name = "srdu01";
            OdbcCommand Odbc12 = new OdbcCommand();
            Odbc12.CommandText = "insert INTO B_BiosynthesisFiles (B_fileID,BXML_ID,B_filename, B_importedpath, B_fileformat, B_filecontent, B_filedata, B_importuser, B_header_xml, B_importtimestamp,B_confidence) VALUES ('" + keys4 + "','" + key_name4 + "','" + filename + "','" + path + "','" + extension + "','',?,'" + name + "','" + header + "','" + Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd,h:mm:tt") + "', '" + confidence1 + "') ";
            Odbc12.Parameters.AddWithValue("@B_filedata", photo);
            try
            {
                Odbc12.Connection = OdbcCon;
                Odbc12.ExecuteNonQuery();
            }
            catch (Exception fds)
            {
                MessageBox.Show(fds.ToString() + "              files");
            }
            XmlNode headerNod;
            string keys = "", keys_name = "";
          
          
            try
            {
                OdbcCommand Odbcq9998 = new OdbcCommand();
                //Odbcq9998.CommandText = "SELECT MAX(Next_BSRC_key) FROM  NEXT_id";
                Odbcq9998.CommandText = "select max(Next_value) from Mxbase_Nextid where NEXT_id = 'NEXT_BSRCkey'";
                Odbcq9998.Connection = OdbcCon;
                DataSet e1 = new DataSet();
                OdbcDataAdapter Odbce1 = new OdbcDataAdapter(Odbcq9998);
                Odbce1.Fill(e1, "Mxbase_Nextid");
                foreach (DataRow row1 in e1.Tables[0].Rows)
                {

                    for (int i1 = 0; i1 < e1.Tables[0].Columns.Count; i1++)
                        keys = row1[i1].ToString();
                }
                if ((keys == "") || (keys == "0"))
                {
                    keys = "1";
                }
                else
                {
                   int w = int.Parse(keys);
                    keys = (w + 1).ToString();
                }
            }
            catch
            {
            }
            BSRCID = "BSRC" + keys;
     
            keys = Regex.Match(BSRCID, @"\d+").Value;

            OdbcCommand dbc1 = new OdbcCommand();
            //dbc1.CommandText = "update NEXT_id set NEXT_BSRC_Key= ('" + keys + "') where Nextkey = 1";
            dbc1.CommandText = "update Mxbase_Nextid set Next_value= ('" + keys + "') where NEXT_id = 'NEXT_BSRCkey'";
            dbc1.Connection = OdbcCon;
            try
            {

                dbc1.ExecuteNonQuery();
            }
            catch (Exception fds)
            {
                MessageBox.Show(fds.ToString());
            }
            string keys3 = "", keys_name3 = "";


            string id = "", genelist1 = "";
            string u = "1000";
            OdbcCommand Odbc1 = new OdbcCommand();


            headerNod = doc.SelectSingleNode("/root/genelist");


            XmlNodeList geneNodelist = doc.SelectNodes("/root/genelist/gene");
            string geneName = ""; int geneID = 0; String translation = "";
            for (int i = 1; i <= geneNodelist.Count; i++)
            {
                richTextBox1.Text = i.ToString();
                geneID = i;
                headerNod = doc.SelectSingleNode("/root/genelist/gene[@id = '" + i + "']/gene_name");
                geneName = headerNod.InnerText;

                headerNod = doc.SelectSingleNode("/root/genelist/gene[@id = '" + i + "']/gene_qualifiers/qualifier [@name='translation']");
                StringBuilder builder = new StringBuilder();
                try
                {
                    if (headerNod.InnerText == null || headerNod.InnerText.Trim().Length <= 0)
                    {

                    }
                    else
                    {
                        builder.Append(headerNod.InnerText);
                    }
                }
                catch
                {

                }
                if (builder.Length > 0)
                {
                    Odbc1.CommandText = "insert INTO B_GeneList (BSRC_ID, b_protein, b_geneID, b_genename) VALUES ('" + BSRCID + "','" + builder + "','" + i + "','" + geneName + "' )";
                    try
                    {
                        Odbc1.Connection = OdbcCon;
                        Odbc1.ExecuteNonQuery();
                    }
                    catch (Exception fds)
                    {
                        MessageBox.Show(i + "                           " + path4file + " B_GEneList                      " + fds.ToString());
                    }

                }
            }
            headerNod = doc.SelectSingleNode("/root/genelist");
            //StringBuilder builder1 = new StringBuilder();
            //try
            //{
            //    if (headerNod.OuterXml == null || headerNod.OuterXml.Trim().Length <= 0)
            //    {

            //    }
            //    else
            //    {
            //        builder1.Append(headerNod.OuterXml);
            //    }
            //}
            //catch
            //{

            //}
          //  MessageBox.Show(headerNod.OuterXml);


            //  richTextBox2.Text = headerNod.OuterXml.Replace("'", "");
            Odbc1.CommandText = "insert INTO B_BiosynthesisList (BSRC_ID, B_sourcefile, B_genelist_xml, B_user, B_timestamp,SEQU_key) VALUES ('" + BSRCID + "','" + key_name4 + "','" + headerNod.OuterXml.Replace("'", "") + "','" + name + "','" + Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd,h:mm:tt") + "') ";
            try
            {
                Odbc1.Connection = OdbcCon;
                Odbc1.ExecuteNonQuery();
            }
            catch (Exception fds)
            {
                MessageBox.Show(path4file + " B_BiosynthesisList                      " + fds.ToString());
            }

            XmlNodeList nodelist2 = doc.SelectNodes("/root/model");

            mods.Clear();

            Dictionary<string, List<int>> mnodes = new Dictionary<string, List<int>>();
            string buildingblocklist = "", geneid = "", func = "";
            string model = "", title = "", confidence = "", generator = "", label = "", organism = "", compound = "", genecluster = "";

            try
            {

                headerNod = doc.SelectSingleNode("/root/motiflist");
                string motifs = headerNod.OuterXml;


                OdbcCommand Odbc0 = new OdbcCommand();
                Odbc0.CommandText = "insert INTO B_BiosynthesisMotifs (BSRC_ID, B_motiflist) VALUES ('" + BSRCID + "','" + motifs.Replace("'", "") + "' ) ";
                try
                {
                    Odbc0.Connection = OdbcCon;
                    Odbc0.ExecuteNonQuery();
                }
                catch (Exception fds)
                {
                    MessageBox.Show("motids             " + fds.ToString());
                }
            }
            catch { }
            string id1 = "", B_model = "", B_model_prev = "", seqids = "";



            XmlNodeList nodelist = doc.SelectNodes("/root/domainlist/domain");
            for (int i = 1; i <= nodelist.Count; i++)
            {
                string active = "";
                headerNod = doc.SelectSingleNode("/root/domainlist/domain[@id = '" + i + "']/nodeid");
                id1 = headerNod.InnerText;
                // MessageBox.Show(id);
                genelist = "";
                headerNod = doc.SelectSingleNode("/root/model/nodelist/node[@id = '" + id1 + "']/class");
                class_xml = headerNod.InnerText;

                headerNod = doc.SelectSingleNode("/root/model/nodelist/node[@id = '" + id1 + "']/context");
                context = headerNod.InnerText;
                buildingblocklist = "";


                try
                {
                    ////applications/application[@mode='3' and not(@tool)]/@name j
                    XmlNode buildingblock1 = doc.SelectSingleNode("/root/model[./nodelist/node[@id = '" + id1 + "']]/@id");
                    //  MessageBox.Show(buildingblock1.Value + " dsa       " + id1);

                    B_model = buildingblock1.Value;
                    int mxID = 0, c_id = 0, f_id = 0;
                    if (!B_model_prev.Trim().Equals(B_model.Trim()))
                    {
                        B_model_prev = B_model;
                        model = B_model.ToString().Trim();

                        headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/title");
                        title = headerNod.InnerText;
                        headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/confidence");
                        confidence = headerNod.InnerText;
                        headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/generator");
                        generator = headerNod.InnerText;
                        headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/label");
                        label = headerNod.InnerText;
                        headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/organism");
                        organism = headerNod.OuterXml;
                        headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/compound");
                        compound = headerNod.OuterXml;
                        headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/genecluster");
                        genecluster = headerNod.OuterXml;

                        XmlNodeList headerNod1 = doc.SelectNodes("/root/model[@id = '" + B_model + "']/organism/identifier[contains(translate(./@source,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'myxobase')]/text()");

                        foreach (XmlNode xn in headerNod1)
                        {
                            if (xn.InnerText.Trim().Contains("key-"))
                            {
                                try
                                {
                                    string[] keyvalues = xn.InnerText.Trim().Split('-');
                                    mxID = Int32.Parse(keyvalues[1].Trim());
                                }
                                catch(Exception ds){
                                    mxID = 0;
                                //    MessageBox.Show(ds.ToString());
                                }
                            }
                        }
                        headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/genecluster/sequence");


                        seqids = headerNod.InnerText;

                        XmlNodeList headerNod2 = doc.SelectNodes("/root/model[@id = '" + B_model + "']/compound/identifier[contains(translate(./@source,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'myxobase')]/text()");

                        foreach (XmlNode xn in headerNod2)
                        {
                            if (xn.InnerText.Trim().Contains("key-"))
                            {
                              //  MessageBox.Show(xn.InnerText.Trim());
                                string[] keyvalues = xn.InnerText.Trim().Split('-');
                                c_id = Int32.Parse(keyvalues[1].Trim());

                            }
                            else if (xn.InnerText.Trim().Contains("family-"))
                            {
                                string[] keyvalues = xn.InnerText.Trim().Split('-');
                                f_id = Int32.Parse(keyvalues[1].Trim());

                            }
                        }


                        string module = "", modulenode = "", mlabel = "";
                        mnodes.Clear();
                        try
                        {

                            headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/modulelist");
                            module = headerNod.OuterXml;
                            XmlNodeList modulelist2 = doc.SelectNodes("/root/model[@id = '" + B_model + "']/modulelist/module");


                            for (int k = 0; k < modulelist2.Count; k++)
                            {



                                modulenode = modulelist2[k].ChildNodes[1].InnerText;

                                string[] j = modulenode.Split('-');
                                List<int> lst = new List<int>();

                                foreach (string k2 in j)
                                {
                                    lst.Add(Convert.ToInt32(k2.Trim()));
                                }

                                headerNod = doc.SelectSingleNode("/root/model[@id = '" + B_model + "']/modulelist/module[@id = '" + k + "']/label");
                                mlabel = modulelist2[k].ChildNodes[0].InnerText;

                                mnodes.Add(mlabel, lst);
                            }
                        }
                        catch (Exception fds)
                        {
                            //  MessageBox.Show(fds.ToString());
                        }
                        XmlNodeList nodelist_module = doc.SelectNodes("/root/model[@id = '" + B_model + "']/modulelist/module");
                        String lab = "", nodelist1 = "";
                        // MessageBox.Show(nodelist_module.Count.ToString());

                        for (int m1 = 0; m1 < nodelist_module.Count; m1++)
                        {


                            //  MessageBox.Show(nodelist_module[m1].ChildNodes[0].Name + "             " + nodelist_module[m1].ChildNodes[0].InnerText);


                            lab = nodelist_module[m1].ChildNodes[0].InnerText;
                            nodelist1 = nodelist_module[m1].ChildNodes[1].InnerText;
                            mods.Add(lab, nodelist1);

                        }

                        try
                        {


                            OdbcCommand Odbcq9998 = new OdbcCommand();
                            //Odbcq9998.CommandText = "SELECT MAX(Next_BSYN_key) FROM  NEXT_id";
                            Odbcq9998.CommandText = "select max(Next_value) from Mxbase_Nextid where NEXT_id = 'Next_BYSNkey'";
                            Odbcq9998.Connection = OdbcCon;
                            DataSet e1 = new DataSet();
                            OdbcDataAdapter Odbce1 = new OdbcDataAdapter(Odbcq9998);
                            Odbce1.Fill(e1, "Mxbase_Nextid");
                            foreach (DataRow row1 in e1.Tables[0].Rows)
                            {

                                for (int i1 = 0; i1 < e1.Tables[0].Columns.Count; i1++)
                                    keys = row1[i1].ToString();
                            }
                            if ((keys == "") || (keys == "0"))
                            {
                                keys = "1";
                            }
                            else
                            {
                                int w = int.Parse(keys);
                                keys = (w + 1).ToString();
                            }
                        }
                        catch
                        {

                        }
                        keys_name = "BSYN" + keys;
                        OdbcCommand Odbc13 = new OdbcCommand();
                        //Odbc13.CommandText = "update NEXT_id set NEXT_BSYN_Key= ('" + keys + "') where Nextkey = 1";
                        Odbc13.CommandText = "update Mxbase_Nextid set Next_value= ('" + keys + "')  where NEXT_id = 'Next_BYSNkey'";
                        Odbc13.Connection = OdbcCon;
                        try
                        {

                            Odbc13.ExecuteNonQuery();
                        }
                        catch (Exception fds)
                        {
                            MessageBox.Show(fds.ToString());
                        }
                        if (mxID == 0)
                        {
                            try
                            {
                                headerNod = doc.SelectSingleNode("/root/Sequencelist/source[@id = 1]/db_xref");
                                string dbxref = headerNod.InnerText;
                                //   MessageBox.Show(dbxref);
                                string[] myxoid = dbxref.Split(':');
                                mxID = Int32.Parse(myxoid[1].Trim());
                            }
                            catch { }
                        }
                        seqdictionary.Add(Int32.Parse(model.Trim()), keys_name);
                        OdbcCommand Odbc11 = new OdbcCommand();
                        Odbc11.CommandText = "insert INTO B_BiosynthesisModels (BSRC_ID,BSYN_ID, B_model, B_title, B_confidence, B_generator, B_label,B_organism, B_compound, B_genecluster,B_modulelist,s_MxID, c_id,f_id) VALUES ('" + BSRCID + "','" + keys_name + "','" + model + "','" + title + "','" + confidence + "','" + generator + "','" + label + "','" + organism + "','" + compound + "','" + genecluster + "' ,'" + module + "'," + mxID + "," + c_id + "," + f_id + ") ";
                        try
                        {
                            Odbc11.Connection = OdbcCon;
                            Odbc11.ExecuteNonQuery();
                        }
                        catch (Exception fds)
                        {
                            MessageBox.Show(fds.ToString() + "                 models");
                        }



                    }
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.ToString().Trim() + " dsa");
                }

                XmlNodeList nodelist_bb = doc.SelectNodes("/root/model/nodelist/node[@id = '" + id1 + "']/buildingblock/moiety");

                string modulelabel = "";
                headerNod = doc.SelectSingleNode("/root/domainlist/domain[@id = '" + id1 + "']");
                // MessageBox.Show(headerNod.OuterXml);
                domain = headerNod.OuterXml;


                headerNod = doc.SelectSingleNode("/root/domainlist/domain[@id = '" + id1 + "']/function");
                func = headerNod.InnerText;
                headerNod = doc.SelectSingleNode("/root/domainlist/domain[@id = '" + id1 + "']/location/gene/geneid");
                geneid = headerNod.InnerText;


                headerNod = doc.SelectSingleNode("/root/domainlist/domain[@id = '" + id1 + "']/dstatus");
                active = headerNod.InnerText;
                string modulename = "";
                foreach (KeyValuePair<string, List<int>> item in mnodes)
                {

                    string key = item.Key;
                    List<int> k = item.Value;
                    if (k.Contains(Convert.ToInt32(id1.Trim())))
                    {
                        modulename = key;
                    }

                }

                try
                {

                    headerNod = doc.SelectSingleNode("/root/model/nodelist/node[@id = '" + id1 + "']/buildingblock");
                    buildingblocklist = headerNod.OuterXml;
                }
                catch { }
                OdbcCommand Obc11 = new OdbcCommand();
                Obc11.CommandText = "insert INTO B_BiosyntheticPathways (BSYN_ID, B_model, B_node_ID, B_class, B_context, B_domain_xml, B_buildingblock_xml, B_gene,B_domain_ID,B_domian,B_active,B_module) VALUES ('" + keys_name + "','" + B_model + "','" + id1 + "','" + class_xml + "','" + context + "','" + domain + "','" + buildingblocklist + "','" + geneid + "','" + i + "', '" + func + "', '" + active + "', '" + modulename + "') ";
                try
                {
                    Obc11.Connection = OdbcCon;
                    Obc11.ExecuteNonQuery();

                }
                catch (Exception fds)
                {
                    MessageBox.Show(fds.ToString() + "                    B_BiosyntheticPathways");
                }

            }
            XmlNodeList seqlist = doc.SelectNodes("/root/Sequencelist/source");
            for (int p = 1; p <= seqlist.Count; p++)
            {


                XmlNode sequ = doc.SelectSingleNode("/root/Sequencelist/source[@id = '" + p + "']/Sequence");


                //   MessageBox.Show(genelist1);

                try
                {
                    OdbcCommand Odbcq998 = new OdbcCommand();
                    //Odbcq998.CommandText = "SELECT MAX(NEXT_SEQU_Key) FROM  NEXT_id";
                    Odbcq998.CommandText = "select max(Next_value) from Mxbase_Nextid where NEXT_id = 'Next_SEQUkey'";
                    Odbcq998.Connection = OdbcCon;
                    DataSet e11 = new DataSet();
                    OdbcDataAdapter Odbce11 = new OdbcDataAdapter(Odbcq998);
                    Odbce11.Fill(e11, "Mxbase_Nextid");
                    foreach (DataRow row1 in e11.Tables[0].Rows)
                    {

                        for (int i1 = 0; i1 < e11.Tables[0].Columns.Count; i1++)
                            keys3 = row1[i1].ToString();
                    }
                    if ((keys3 == "") || (keys3 == "0"))
                    {
                        keys3 = "1";
                    }
                    else
                    {

                        int w = int.Parse(keys3);
                        keys3 = (w + 1).ToString();
                    }
                }
                catch (Exception fds)
                {
                    MessageBox.Show(fds.ToString());
                }

                keys_name3 = "SEQU" + keys3;
                string bsynkey = "";
                try
                {
                    if (seqdictionary.ContainsKey(p))
                    {
                        bsynkey = seqdictionary[p];
                    }
                    else
                    {
                        bsynkey = "";
                    }
                }
                catch { }
                OdbcCommand Odbc = new OdbcCommand();
                Odbc.CommandText = "insert INTO B_Sequences (BSRC_ID,SEQ_sourceID,SEQU_ID, SEQ_format, SEQ_content, SEQ_type, SEQ_information, SEQ_user, SEQ_timestamp) VALUES ('" + BSRCID + "','" + p + "','" + keys_name3 + "','Fasta','DNA','xml','" + sequ.InnerText + "','" + name.Trim() + "','" + Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd,h:mm:tt") + "' ) ";


                Odbc.Connection = OdbcCon;
                try
                {
                    Odbc.ExecuteNonQuery();

                }
                catch (Exception fds)
                {
                    MessageBox.Show(fds.ToString());
                }

                OdbcCommand Odbc14 = new OdbcCommand();
                //Odbc14.CommandText = "update NEXT_id set NEXT_SEQU_Key= ('" + keys3 + "') where Nextkey = 1";
                Odbc14.CommandText = "update Mxbase_Nextid set Next_value= ('" + keys3 + "')  where NEXT_id = 'Next_SEQUkey'";
                Odbc14.Connection = OdbcCon;
                try
                {
                    Odbc14.ExecuteNonQuery();
                }
                catch (Exception fds)
                {
                    MessageBox.Show(fds.ToString());
                }


            }

        }
        private void button1_Click(object sender, EventArgs e)
        {

            string[] filePaths = Directory.GetFiles(textBox1.Text);
            foreach (string j in filePaths)
            {
                try
                {
                    xmlimport(j);
                    //MessageBox.Show(j);
                }
                catch (Exception ds)
                {
                    MessageBox.Show(j+"                          "+ds.ToString());
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
             
            }
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //int k = 0;

            //panel2.Controls.Clear();
            //string[] FirstBox = richTextBox1.Lines;
            //string[] SecondBox = richTextBox3.Lines;
            //int margin = 5;
            //Point pt = new Point(margin, margin);
            //int width = panel2.Width;
            ////  Graphics g = panel2.CreateGraphics();
            //StringFormat sf = new StringFormat();
            //// sf.Alignment = StringAlignment.Center;
            //Rectangle cr, cr1;
            //String a = "sdasadsadsadsadsaSASDSDASDS";
            //Font courier = new Font(FontFamily.GenericMonospace, 9.0F);

            //int y = 0, w = 20, h = 20, ytarget = 25;
            //for (int i = 0; i <= Math.Max(FirstBox.Count(), SecondBox.Count()) - 1; i++)
            //{
            //    List<string> seriesArray = new List<string>();
            //    List<int> pointsArray = new List<int>();
            //    int x = 0;
            //    string first = "", second = "";
            //    try
            //    {
            //        first = FirstBox[i];
            //    }
            //    catch { first = ""; }
            //    try
            //    {
            //        second = SecondBox[i];
            //    }
            //    catch { second = ""; }

            //    //var firstWithSpaces = first.Aggregate(string.Empty, (c, l) => c + l + ' ').PadRight(2);
            //    //var secondWithSpaces = second.Aggregate(string.Empty, (c, l) => c + l + ' ').PadRight(2);
            //    Chart chart2 = new Chart();
            //    cr = new Rectangle(x, y, w + 40, h); //same as the colored region
            //    cr1 = new Rectangle(x, ytarget, w + 40, h); //same as the colored region
            //    x = x + 10;
            //    w = w + 10;
            //    SolidBrush myBrush = new SolidBrush(Color.Gray);
            //    e.Graphics.DrawString("BSYN1", courier, myBrush, cr, sf);
            //    int space = 15;

            //    e.Graphics.DrawString("BSYN2", courier, myBrush, cr1, sf);
            //    x = x + 40;
            //    for (int q = 0; q < first.Count(); q++)
            //    {
            //        char qy = '1'; char ty = '2';

            //        try
            //        {
            //            qy = first[q];
            //        }
            //        catch { qy = '1'; }
            //        try
            //        {
            //            ty = second[q];
            //        }
            //        catch { ty = '2'; }



            //        if (qy.ToString().Trim().Length > 0 && ty.ToString().Trim().Length > 0)
            //        {

            //            if (qy == ty)
            //            {

            //                cr = new Rectangle(x, y, w, h); //same as the colored region
            //                cr1 = new Rectangle(x, ytarget, w, h); //same as the colored region
            //                x = x + space;
            //                w = w + 20;
            //                myBrush = new SolidBrush(Color.Blue);
            //                e.Graphics.DrawString(qy.ToString(), courier, myBrush, cr, sf);
            //                //SolidBrush blueBrush = new SolidBrush(Color.Red);
            //                //e.Graphics.FillRectangle(blueBrush, cr);

            //                e.Graphics.DrawString(ty.ToString(), courier, myBrush, cr1, sf);
            //            }

            //            else if (!qy.ToString().Equals("-") && !ty.ToString().Equals("-"))
            //            {
            //                cr = new Rectangle(x, y, w, h); //same as the colored region
            //                cr1 = new Rectangle(x, ytarget, w, h); //same as the colored region
            //                x = x + space;
            //                w = w + 20;
            //                myBrush = new SolidBrush(Color.Green);
            //                e.Graphics.DrawString(qy.ToString(), courier, myBrush, cr, sf);


            //                e.Graphics.DrawString(ty.ToString(), courier, myBrush, cr1, sf);
            //            }
            //            else
            //            {

            //                cr = new Rectangle(x, y, w, h); //same as the colored region
            //                cr1 = new Rectangle(x, ytarget, w, h); //same as the colored region
            //                x = x + space;
            //                w = w + 20;
            //                myBrush = new SolidBrush(Color.Red);
            //                e.Graphics.DrawString(qy.ToString(), courier, myBrush, cr, sf);
            //                e.Graphics.DrawString(ty.ToString(), courier, myBrush, cr1, sf);
            //            }
            //        }
            //    }



            //    barChart = new Chart();

            //    //   this.components = new System.ComponentModel.Container();
            //    ChartArea chartArea1 = new ChartArea();
            //    Legend legend1 = new Legend() { BackColor = Color.Green, ForeColor = Color.Black, Title = "" };
            //    Legend legend2 = new Legend() { BackColor = Color.Green, ForeColor = Color.Black, Title = "" };

            //    ((ISupportInitialize)(barChart)).BeginInit();

            //    SuspendLayout();


            //    //====Bar Chart
            //    chartArea1 = new ChartArea();
            //    barChart.ChartAreas.Add(chartArea1);
            //    barChart.Legends.Add(legend2);

            //    AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //    AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //    //this.ClientSize = new System.Drawing.Size(284, 262);           
            //    this.Load += new EventHandler(Form1_Load);

            //    ((ISupportInitialize)(barChart)).EndInit();

            //    barChart.Width = x;
            //    barChart.Height = 20;

            //    double[] xData = new double[pointsArray.Count];
            //    double[] yData = new double[pointsArray.Count];
            //    Series series = new Series
            //    {
            //        Name = "ds",
            //        IsVisibleInLegend = false,
            //        ChartType = SeriesChartType.Column
            //    };
            //    int[] values = new int[pointsArray.Count];

            //    for (int j = 0; j < pointsArray.Count; j++)
            //    {
            //        // chart2.Series["test1"].Points.AddXY                          (j, pointsArray[j]);
            //        xData[j] = j;
            //        yData[j] = j;
            //        series.Points.Add(pointsArray[j]);
            //    }

            //    //  barChart.ChartAreas[0].wi = "1";
            //    barChart.Palette = ChartColorPalette.Fire;
            //    barChart.ChartAreas[0].BackColor = Color.Transparent;
            //    barChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            //    barChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            //    barChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            //    barChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            //    barChart.ChartAreas[0].AxisY.LineWidth = 0;
            //    barChart.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
            //    barChart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            //    barChart.Series.Add(series);
            //    barChart.Series["ds"]["PointWidth"] = "1";
            //    barChart.Location = new Point(2, y = y + 65);


            //  //  panel2.Controls.Add(barChart);
            //    y = y + 65;
            //    ytarget = ytarget + 65;
            //    x = 0;
            //}
            //e.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);


            //base.OnPaint(e);
            //l.Location = new Point(0, y + 65);
            //panel2.AutoScrollMinSize = new Size(0, y + 65);
            ////     panel2.Refresh();
            ////panel2.Invalidate();
            //panel2.Controls.Add(l);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Label l = new Label();

            int k = 0;

            panel2.Controls.Clear();
            string[] FirstBox = richTextBox1.Lines;
            string[] SecondBox = richTextBox3.Lines;
            int margin = 5;
            Point pt = new Point(margin, margin);
            int width = panel2.Width;
              Graphics g = panel2.CreateGraphics();
            StringFormat sf = new StringFormat();
            // sf.Alignment = StringAlignment.Center;
            Rectangle cr, cr1;
            String a = "sdasadsadsadsadsaSASDSDASDS";
            Font courier = new Font(FontFamily.GenericMonospace, 8.0F);
          
            int y = 0, w = 10, h = 20, ytarget = 25;
            for (int i = 0; i <= Math.Max(FirstBox.Count(), SecondBox.Count()) - 1; i++)
            {
                List<string> seriesArray = new List<string>();
                List<double> pointsArray = new List<double>();
                int x = 0;
                string first = "", second = "";
                try
                {
                    first = FirstBox[i];
                }
                catch { first = ""; }
                try
                {
                    second = SecondBox[i];
                }
                catch { second = ""; }

                //var firstWithSpaces = first.Aggregate(string.Empty, (c, l) => c + l + ' ').PadRight(2);
                //var secondWithSpaces = second.Aggregate(string.Empty, (c, l) => c + l + ' ').PadRight(2);

                cr = new Rectangle(x, y, w + 30, h); //same as the colored region
                cr1 = new Rectangle(x, ytarget, w + 30, h); //same as the colored region
                x = x + 10;
                w = w + 10;
                SolidBrush myBrush = new SolidBrush(Color.Gray);
                //g.DrawString("BSYN1", courier, myBrush, cr, sf);


                //g.DrawString("BSYN2", courier, myBrush, cr1, sf);
                x = x + 40;
                for (int q = 0; q < first.Count(); q++)
                {
                    char qy = '1'; char ty = '2';

                    try
                    {
                        qy = first[q];
                    }
                    catch { qy = '1'; }
                    try
                    {
                        ty = second[q];
                    }
                    catch { ty = '2'; }



                    if (qy.ToString().Trim().Length > 0 && ty.ToString().Trim().Length > 0)
                    {

                        if (qy == ty)
                        {
                            pointsArray.Add(1);
                            cr = new Rectangle(x, y, w, h); //same as the colored region
                            cr1 = new Rectangle(x, ytarget, w, h); //same as the colored region
                            x = x + 10;
                            w = w + 10;
                            myBrush = new SolidBrush(Color.Blue);
                            //g.DrawString(qy.ToString(), courier, myBrush, cr, sf);

                            //g.DrawString(ty.ToString(), courier, myBrush, cr1, sf);
                        }

                        else if (!qy.ToString().Equals("-") && !ty.ToString().Equals("-"))
                        {
                            pointsArray.Add(0.5);
                            cr = new Rectangle(x, y, w, h); //same as the colored region
                            cr1 = new Rectangle(x, ytarget, w, h); //same as the colored region
                            x = x + 10;
                            w = w + 10;
                            myBrush = new SolidBrush(Color.Green);
                            //g.DrawString(qy.ToString(), courier, myBrush, cr, sf);


                            //g.DrawString(ty.ToString(), courier, myBrush, cr1, sf);
                        }
                        else
                        {
                            pointsArray.Add(0);
                            cr = new Rectangle(x, y, w, h); //same as the colored region
                            cr1 = new Rectangle(x, ytarget, w, h); //same as the colored region
                            x = x + 10;
                            w = w + 10;
                            myBrush = new SolidBrush(Color.Red);
                            //g.DrawString(qy.ToString(), courier, myBrush, cr, sf);
                            //g.DrawString(ty.ToString(), courier, myBrush, cr1, sf);
                        }
                    }
                }

                Chart barChart;

                barChart = new Chart();

                //   this.components = new System.ComponentModel.Container();
                ChartArea chartArea1 = new ChartArea();
                Legend legend1 = new Legend() { BackColor = Color.Green, ForeColor = Color.Black, Title = "" };
                Legend legend2 = new Legend() { BackColor = Color.Green, ForeColor = Color.Black, Title = "" };

                ((ISupportInitialize)(barChart)).BeginInit();

                SuspendLayout();


                //====Bar Chart
                chartArea1 = new ChartArea();
                barChart.ChartAreas.Add(chartArea1);
                barChart.Legends.Add(legend2);

                AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                //this.ClientSize = new System.Drawing.Size(284, 262);           
                this.Load += new EventHandler(Form1_Load);

                ((ISupportInitialize)(barChart)).EndInit();

                barChart.Width = x;
                barChart.Height = 20;

                double[] xData = new double[pointsArray.Count];
                double[] yData = new double[pointsArray.Count];
                Series series = new Series
                {
                    Name = "ds",
                    IsVisibleInLegend = false,
                    ChartType = SeriesChartType.Column
                };
                int[] values = new int[pointsArray.Count];

                for (int j = 0; j < pointsArray.Count; j++)
                {
                    // chart2.Series["test1"].Points.AddXY                          (j, pointsArray[j]);
                    xData[j] = j;
                    yData[j] = j;
                    series.Points.Add(pointsArray[j]);
                }

                //  barChart.ChartAreas[0].wi = "1";
                barChart.Palette = ChartColorPalette.Fire;
                barChart.ChartAreas[0].BackColor = Color.Transparent;
                barChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                barChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                barChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                barChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                barChart.ChartAreas[0].AxisY.LineWidth = 0;
                barChart.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
                barChart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
                barChart.Series.Add(series);
                barChart.Series["ds"]["PointWidth"] = "1";
                barChart.Location = new Point(2,y = y+65);
               
             
                panel2.Controls.Add(barChart);
                barChart.Invalidate();
                panel2.Invalidate();
                y = y + 145;
                ytarget = ytarget + 145;
                x = 0;
            }
         //   g.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

            
          //  l.Location = new Point(0, y + 65);
         //   panel2.AutoScrollMinSize = new Size(0, y + 65);
        
         //   panel2.Refresh();
         //   

        }
        private StringBuilder getData()
        {
            StringBuilder sd = new StringBuilder();
            try
            {
                DataSet z2311 = new DataSet();
                OdbcDataAdapter adaper2311 = new OdbcDataAdapter("SELECT b_alignment FROM B_GeneComparisons where b_process_ID = '92f23c432e9e402b90e5df681829c7c5' and b_query_BSRCID = 'BSRC3' and b_target_BSRCID = 'BSRC9' and b_querygeneID = '1' and b_targetgeneID= '328,329,331'", OdbcCon);
                adaper2311.Fill(z2311);
                // textBox30.Clear();
                foreach (DataRow row1 in z2311.Tables[0].Rows)
                {

                    for (int i1 = 0; i1 < z2311.Tables[0].Columns.Count; i1++)
                    {
                      //    MessageBox.Show(row1[i1].ToString());
                        richTextBox1.Text = (row1[i1].ToString().Trim());
                        sd.Append(row1[i1].ToString());
                    }
                }
            }
            catch(Exception ds){
                MessageBox.Show(ds.ToString());
            }
            return sd;
        }
       



        public void readFastaSequencesFromFile()
        {
            StringBuilder sd = getData();
            List<String> desc = new List<String>();
            List<String> seq = new List<String>();
            try
            {
                StringBuilder buffer = new StringBuilder();
                String[] d = sd.ToString().Split('\n');
                foreach (string line in richTextBox3.Lines)
                {
                  //  MessageBox.Show(line);
                    if (line.Length > 0 && line[0] == '>')
                    {
                        if (buffer.ToString().Trim().Length > 1)
                        {
                            seq.Add(buffer.ToString());
                        }
                        buffer = new StringBuilder();
                        desc.Add(line);
                    }
                    else
                    {
                        buffer.Append(line.Trim());
                    }
                }
                if (buffer.Length != 0)
                {
                    seq.Add(buffer.ToString());
                }

            }
            catch (IOException e)
            {


            }
            description = new String[desc.Count];
            sequence1 = new String[seq.Count];
            for (int i = 0; i < seq.Count; i++)
            {
                if (seq[i].Trim().Length > 1)
                {
                   description[i] = (String)desc[i];
                    sequence1[i] = (String)seq[i];
                }
            }

        }
        public static List<double> getIdentityAndSimilarityScores(String[] alignments)
        {
            String symbol = "";
            List<double> pointsArray = new List<double>();
            int score = 0;
            int penalty = -4;
            int nb_column = alignments[0].Length;
            int nb_row = alignments.Length;
            if (alignments.Length > 0)
            {
                for (int c = 0; c < nb_column; ++c)
                {

                    for (int r = 0; r < nb_row - 1; ++r)
                    {
                        for (int s = r + 1; s < nb_row; ++s)
                        {

                            String align1 = alignments[r];
                            String align2 = alignments[s];

                            if (align1[c] == align2[c] && align1[c] != '-' && align2[c] != '-')
                            {
                                symbol = symbol + align1[c];
                                pointsArray.Add(1);
                                score = score + BLOSUM.getDistance(align1[c], align2[c]);
                            }
                            else if (align1[c] != align2[c] && align1[c] != '-' && align2[c] != '-')
                            {
                                score = score + BLOSUM.getDistance(align1[c], align2[c]);
                                if (BLOSUM.getDistance(align1[c], align2[c]) > 0)
                                {
                                    pointsArray.Add(0.5);
                                }
                                else
                                {
                                    pointsArray.Add(0);
                                }
                            }
                            else if (align1[c] == '-' || align2[c] == '-')
                            {
                                pointsArray.Add(0);
                            }
                        }
                    }

                }
            }
            return pointsArray;
        }
        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
        static public int MeasureDisplayStringWidth(Graphics graphics, string text, Font font)
        {
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0, 0, 1000, 1000);
            var ranges = new System.Drawing.CharacterRange(0, text.Length);
            System.Drawing.Region[] regions = new System.Drawing.Region[1];

            //  format.SetMeasurableCharacterRanges(ranges);

            regions = graphics.MeasureCharacterRanges(text, font, rect, format);
            rect = regions[0].GetBounds(graphics);

            return (int)(rect.Right + 1.0f);
        }
        private void button4_Click(object sender, EventArgs e)
        {
        readFastaSequencesFromFile();
        String[] alignments = sequence1;
            int k = 0;
            int length = 80;
            panel2.Controls.Clear();
            //string[] FirstBox = richTextBox1.Lines;
            //string[] SecondBox = richTextBox3.Lines;
            int margin = 5;
            Point pt = new Point(margin, margin);
            int width = panel2.Width;
         //   MessageBox.Show(FirstBox.Count().ToString());

            List<IEnumerable<string>> list = new List<IEnumerable<string>>();
            int number = 0;
            foreach (String i in alignments)
            {
                list.Add(Split(i, length));
                number = Split(i, length).Count();
            }
            RichTextBox label = new RichTextBox();
            RichTextBox ds = new RichTextBox();
            int count = 0; //var firstWithSpaces = ""; 
            pt.Y = 2;
            foreach (string i in list[0])
            {
                
             
                List<double> pointsArray = new List<double>();
                int nb_column = length;
                int nb_row = alignments.Count();
                for (int c = 0; c < nb_column; ++c)
                {

                    for (int r = 0; r < nb_row - 1; ++r)
                    {
                        String align1 = alignments[r];

                        for (int s = r + 1; s < nb_row; ++s)
                        {

                            ds = new RichTextBox();
                            String align2 = alignments[s];


                            if (align1[c] == align2[c] && align1[c] != '-' && align2[c] != '-')
                            {
                                pointsArray.Add(1);
                            }
                            else if (align1[c] != align2[c] && align1[c] != '-' && align2[c] != '-')
                            {
                                if (BLOSUM.getDistance(align1[c], align2[c]) > 0)
                                {
                                    pointsArray.Add(0.5);
                                }
                                else
                                {
                                    pointsArray.Add(0);
                                }
                            }
                            else if (align1[c] == '-' || align2[c] == '-')
                            {
                                pointsArray.Add(0);
                            }
                        }
                    }

                }





                Chart barChart;

                barChart = new Chart();

                //   this.components = new System.ComponentModel.Container();
                ChartArea chartArea1 = new ChartArea();
                Legend legend1 = new Legend() { BackColor = Color.Green, ForeColor = Color.Black, Title = "" };
                Legend legend2 = new Legend() { BackColor = Color.Green, ForeColor = Color.Black, Title = "" };

                ((ISupportInitialize)(barChart)).BeginInit();

                SuspendLayout();


                //====Bar Chart
                chartArea1 = new ChartArea();
                barChart.ChartAreas.Add(chartArea1);
                barChart.Legends.Add(legend2);

                AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                //this.ClientSize = new System.Drawing.Size(284, 262);           
                this.Load += new EventHandler(Form1_Load);

                ((ISupportInitialize)(barChart)).EndInit();


                barChart.Width = length * 13;
                barChart.Height = 10;

                double[] xData = new double[pointsArray.Count];
                double[] yData = new double[pointsArray.Count];
                Series series = new Series
                {
                    Name = "ds",
                    IsVisibleInLegend = false,
                    ChartType = SeriesChartType.Column
                };
                int[] values = new int[pointsArray.Count];

                for (int j = 0; j < pointsArray.Count; j++)
                {
                    // chart2.Series["test1"].Points.AddXY                          (j, pointsArray[j]);
                    xData[j] = j;
                    yData[j] = j;
                    series.Points.Add(pointsArray[j]);
                }

                //  barChart.ChartAreas[0].wi = "1";
                barChart.Palette = ChartColorPalette.Fire;
                barChart.ChartAreas[0].BackColor = Color.Transparent;
                barChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                barChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                barChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                barChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                barChart.ChartAreas[0].AxisY.LineWidth = 0;
                barChart.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
                barChart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
                barChart.Series.Add(series);
                barChart.Series["ds"]["PointWidth"] = "1";
                pt.X = 2;
               // pt.Y += ds.Height - 30;
                barChart.Location = pt;
                barChart.Invalidate();
                pt.Y += ds.Height-20;
                panel2.Controls.Add(barChart);
                pt.X = 44;
                ds = new RichTextBox();
                number = 0;
                label = new RichTextBox();



                label = new RichTextBox();

                label.Width = 70;
                label.Height = 40;
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                label.BorderStyle = BorderStyle.None;

                Font courier = new Font("Courier New", ds.Font.Size,
                 ds.Font.Style, ds.Font.Unit);
                label.Font = courier;

                label.Location = pt;
                label.Text = "BSYn1" + "\n" + "BSYN2";
                ds.Font = courier;
                ds.Width = 1200;
                ds.Height = alignments.Count() * 16;
                ds.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                ds.Font = courier;
                int totalWidth = pt.X + ds.Width * 2;
                // pt.X = label.Width + margin;
                pt.Y += ds.Height - 20;
                ds.Location = pt;


                foreach (string j in alignments)
                {
                    IEnumerable<string> s = list[number];
                    List<string> asList = s.ToList();
                    try
                    {
                     //  var firstWithSpaces = asList[count].Aggregate(string.Empty, (c, l) => c + l + ' ');
                        if (ds.Text.Length < 1)
                        {

                            ds.AppendText(asList[count]);
                        }
                        else
                        {

                            ds.AppendText("\n" + asList[count]);
                        }
                    }
                    catch { }
                    number = number + 1;
                }
                count = count + 1;
                panel2.Controls.Add(ds);
             //   MessageBox.Show("");
               
            }



            panel2.Invalidate();



       
            //    count = 0;














            //for (int i = 0; i <= sequence1[0].Length; i++)
            //{
            //    List<string> seriesArray = new List<string>();
            //    pointsArray = new List<double>();
            //    string first = "", second = "";
            //    try
            //    {
            //        first = FirstBox[i];
            //    }
            //    catch { first = ""; }
            //    try
            //    {
            //        second = SecondBox[i];
            //    }
            //    catch { second = ""; }
            //  //  MessageBox.Show(first + "               " + second);
            //    // Clipboard.SetText(first + " \n" + second, TextDataFormat.Rtf);

            //    RichTextBox ds = new RichTextBox();
            //    RichTextBox label = new RichTextBox();
               
            //    pt.X = 2;
            //    label.Width = 70;
            //    label.Height = 40;
            //    label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            //    label.BorderStyle = BorderStyle.None;

            //    Font courier = new Font("Courier New", ds.Font.Size,
            //     ds.Font.Style, ds.Font.Unit);
            //    label.Font = courier;
               
            //    label.Location = pt;
            //    label.Text = "BSYn1" + "\n" + "BSYN2";
            //    ds.Font = courier;
            //    ds.Width = 970;
            //    ds.Height = 40;
            //    ds.Anchor = AnchorStyles.Top | AnchorStyles.Left ;

            //    ds.Font = courier;
            //    int totalWidth = pt.X + ds.Width*2;
            //    pt.X = label.Width + margin;



            //    ds.BorderStyle = BorderStyle.None;

            //    ds.Location = pt;
            ////    pt.Y += ds.Height + margin;
            //    var firstWithSpaces = first.Aggregate(string.Empty, (c, l) => c + l + ' ');
            //    var secondWithSpaces = second.Aggregate(string.Empty, (c, l) => c + l + ' ');
            //    ds.Text = firstWithSpaces + " \n" + secondWithSpaces;
            //    panel2.Controls.Add(label);
            //    panel2.Controls.Add(ds);

            //    int stringWidth = 800;

            //    // Measure string.
            //   SizeF stringSize = new SizeF();
            //    Graphics g = ds.CreateGraphics();
            //   stringSize = g.MeasureString(secondWithSpaces, courier, stringWidth);


            // //   int size = MeasureDisplayStringWidth(g,secondWithSpaces,courier);
            //    stringSize = g.MeasureString(secondWithSpaces, courier,ds.Width);
            //   // g.dispose();
            //    //   MessageBox.Show(ds.Lines[0] + "               " + ds.Lines[1]);
            //    //int length = ds.Lines[0].Count(); int count = 0;
            //    for (int q = 0; q < firstWithSpaces.Count(); q++)
            //    {
            //        char qy = '1'; char ty = '2';

            //        try
            //        {
            //            qy = firstWithSpaces[q];
            //        }
            //        catch { qy = '1'; }
            //        try
            //        {
            //            ty = secondWithSpaces[q];
            //        }
            //        catch { ty = '2'; }
            //        //    MessageBox.Show(qy.ToString() + "             " + ty.ToString());
            //        if (qy.ToString().Trim().Length > 0 && ty.ToString().Trim().Length > 0)
            //        {
            //            if (qy == ty)
            //            {
            //                pointsArray.Add(1);

                            
            //            }
            //            else if (qy != '-' && ty != '-')
            //            {
            //                pointsArray.Add(0.5);
                           
            //            }
            //            else
            //            {
            //                pointsArray.Add(0);
            //            }
            //        }
            //    }

            //    //Chart barChart;

            //    //barChart = new Chart();
               
            //    //    //   this.components = new System.ComponentModel.Container();
            //    //    ChartArea chartArea1 = new ChartArea();
            //    //    Legend legend1 = new Legend() { BackColor = Color.Green, ForeColor = Color.Black, Title = "" };
            //    //    Legend legend2 = new Legend() { BackColor = Color.Green, ForeColor = Color.Black, Title = "" };

            //    //    ((ISupportInitialize)(barChart)).BeginInit();

            //    //    SuspendLayout();


            //    //    //====Bar Chart
            //    //    chartArea1 = new ChartArea();
            //    //    barChart.ChartAreas.Add(chartArea1);
            //    //    barChart.Legends.Add(legend2);

            //    //    AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //    //    AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //    //    //this.ClientSize = new System.Drawing.Size(284, 262);           
            //    //    this.Load += new EventHandler(Form1_Load);

            //    //    ((ISupportInitialize)(barChart)).EndInit();

            //    //    barChart.Width = (int)Math.Ceiling(stringSize.Width)+120;
            //    //    barChart.Height = 20;

            //    //    double[] xData = new double[pointsArray.Count];
            //    //    double[] yData = new double[pointsArray.Count];
            //    //    Series series = new Series
            //    //    {
            //    //        Name = "ds",
            //    //        IsVisibleInLegend = false,
            //    //        ChartType = SeriesChartType.Column
            //    //    };
            //    //    int[] values = new int[pointsArray.Count];

            //    //    for (int j = 0; j < pointsArray.Count; j++)
            //    //    {
            //    //        // chart2.Series["test1"].Points.AddXY                          (j, pointsArray[j]);
            //    //        xData[j] = j;
            //    //        yData[j] = j;
            //    //        series.Points.Add(pointsArray[j]);
            //    //    }

            //    //    //  barChart.ChartAreas[0].wi = "1";
            //    //    barChart.Palette = ChartColorPalette.Fire;
            //    //    barChart.ChartAreas[0].BackColor = Color.Transparent;
            //    //    barChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            //    //    barChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            //    //    barChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            //    //    barChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            //    //    barChart.ChartAreas[0].AxisY.LineWidth = 0;
            //    //    barChart.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
            //    //    barChart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            //    //    barChart.Series.Add(series);
            //    //    barChart.Series["ds"]["PointWidth"] = "1";
            //    //    pt.X = 40;
            //    //      pt.Y += ds.Height + margin + margin;
            //    //    barChart.Location = pt;
            //    //    barChart.Invalidate();
               
            //    //panel2.Controls.Add(barChart);
            //    pt.Y += ds.Height + 3 * margin;
            //}




            //    for (int c = 0; c < nb_column; ++c)
            //    {
            //        count = count + 1;
            //        for (int r = 0; r < nb_row - 1; ++r)
            //        {
            //            String align1 = alignments[r];
            //            //    pt.Y += ds.Height + margin;
            //            for (int s = r + 1; s < nb_row; ++s)
            //            {

            //                ds = new RichTextBox();
            //                String align2 = alignments[s];


            //                if (align1[c] == align2[c] && align1[c] != '-' && align2[c] != '-')
            //                {
            //                    symbol = symbol + align1[c];
            //                    pointsArray.Add(1);
            //                    score = score + BLOSUM.getDistance(align1[c], align2[c]);
            //                }
            //                else if (align1[c] != align2[c] && align1[c] != '-' && align2[c] != '-')
            //                {
            //                    score = score + BLOSUM.getDistance(align1[c], align2[c]);
            //                    if (BLOSUM.getDistance(align1[c], align2[c]) > 0)
            //                    {
            //                        pointsArray.Add(0.5);
            //                    }
            //                    else
            //                    {
            //                        pointsArray.Add(0);
            //                    }
            //                }
            //                else if (align1[c] == '-' || align2[c] == '-')
            //                {
            //                    pointsArray.Add(0);
            //                }
            //            }
            //        }

            //    }
          


           
        }
       

        int GetMaxStringLengthPerLine(RichTextBox textbox)
        {
            return GetMaxStringLength(textbox.Size.Width, textbox.Font);
        }

        int GetMaxStringLength(int width, Font font)
        {
            int i = 0;
            while (TextRenderer.MeasureText(new string('A', ++i), font).Width <= width) ;
            return --i;
        }
        private void DrawStringWithCharacterBounds(Graphics gr, string text, Font font, Rectangle rect)
        {
            using (StringFormat string_format = new StringFormat())
            {
                string_format.Alignment = StringAlignment.Center;
                string_format.LineAlignment = StringAlignment.Center;
               

                // Make a CharacterRange for the string's characters.
                List<CharacterRange> range_list =
                    new List<CharacterRange>();
                for (int i = 0; i < text.Length; i++)
                {
                    range_list.Add(new CharacterRange(i, 1));
                }
                string_format.SetMeasurableCharacterRanges(
                    range_list.ToArray());

                // Measure the string's character ranges.
                Region[] regions = gr.MeasureCharacterRanges(
                    text, font, rect, string_format);

                // Draw the character bounds.
                Rectangle[] o = new Rectangle[text.Length];
                gr.DrawString(text.ToString(), font, Brushes.Blue, rect,
                      string_format);
                for (int i = 0; i < text.Length; i++)
                {
                    Rectangle char_rect =
                        Rectangle.Round(regions[i].GetBounds(gr));
                    if (i == 0)
                    {
                    //    gr.FillRectangle(Brushes.Yellow, char_rect);
                    }
                    else
                    {
                      //  gr.FillRectangle(Brushes.Gray, char_rect);
                    }
                    // Draw the string.
                 
                   // gr.DrawRectangle(Pens.Red, char_rect);
                }
                
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int wid = 1000;
            int hgt = ClientSize.Height;
            int y = 0; Rectangle rect;

            rect = new Rectangle(2, 2, wid, hgt);
            using (Font font = new Font("Times New Roman", 4, FontStyle.Italic))
            {
                //MessageBox.Show(seq[0].Substring(2130).Trim()+"                 "+wid);
                DrawStringWithCharacterBounds(e.Graphics, seq[0].Substring(2130).Trim(), font, rect);
            }



            wid /= 2;
            rect = new Rectangle(0, hgt, wid, hgt);
            using (Font font = new Font("Times New Roman", 60, FontStyle.Regular))
            {
                DrawStringWithCharacterBounds(e.Graphics, "jiffy", font, rect);
            }

            rect = new Rectangle(wid, hgt, wid, hgt);
            using (Font font = new Font("Times New Roman", 60, FontStyle.Italic))
            {
                DrawStringWithCharacterBounds(e.Graphics, "jiffy", font, rect);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string JOBID = "f2a9a91df4444dc6b87c0f6ef3da1e4f";
            OdbcCommand m1 = new OdbcCommand();
            m1.CommandText = "select b_process_ID, b_query_BSRCID, b_target_BSRCID, b_querygeneID, b_targetgeneID, b_identical, b_similarity, b_alignment from B_GeneComparisons where b_process_ID = '" + JOBID.Trim() + "' ";

            m1.Connection = OdbcCon;
            DataSet ds4filter = new DataSet();
            OdbcDataAdapter b1 = new OdbcDataAdapter(m1);
            b1.Fill(ds4filter);
            dataGridView1.DataSource = ds4filter.Tables[0]; 
            IEnumerable<DataGridViewRow> rows = null;
            rows =        from DataGridViewRow row in dataGridView1.Rows     where row.Cells["b_target_BSRCID"].Value.ToString().Equals("BSRC22") && row.Cells["b_targetgeneID"].Value.ToString().Contains("16")            select row;
            foreach (DataGridViewRow r in rows)
            {       
               
                  MessageBox.Show(r.Index.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //comboBox17.SelectedIndex = 1;
            //extractSequences();
            NewickParser np = new NewickParser();
            FileStream stream1 = File.Open("C:\\Users\\srdu001\\Desktop\\sdasd.tre", FileMode.Open);
            np.Parse(stream1);
            //np.
        }
        public void extractSequences()
        {
            string path = "C:\\Users\\srdu001\\Desktop\\A.fasta";
            StringBuilder sb = new StringBuilder();
            string query = setQuery();
            DataSet a = new DataSet();
            OdbcDataAdapter adpter = new OdbcDataAdapter(query, OdbcCon);
            adpter.Fill(a);
            foreach (DataRow row in a.Tables[0].Rows)
            {
                for (int i1 = 0; i1 < a.Tables[0].Columns.Count; i1++)
                {
                    sb = new StringBuilder();
                    MessageBox.Show(row[i1].ToString());
                   // sb.Append(row[i1].ToString());
                  //  dumpSequences(path, sb);
                }
            }

        }
        private string  setQuery()
        {
            string query = "";
            if (comboBox17.SelectedIndex == 1)
            {
                query = "SELECT ExtractValue(B_domain_xml, '/domain/location/protein/sequence') FROM B_BiosyntheticPathways where BSYN_ID = '" + bsynid + "' and ExtractValue(B_domain_xml, '//function') = '" + domain + "' and ExtractValue(B_buildingblock_xml, '//name='" + bb + "')')";
            }
            else if (comboBox17.SelectedIndex == 0)
            {
                query = "SELECT ExtractValue(B_domain_xml, '/domain/location/protein/sequence') FROM B_BiosyntheticPathways where BSYN_ID = '" + bsynid + "' and ExtractValue(B_domain_xml, '//function') = '" + domain + "'";
            }
            return query;
        }
        public void dumpSequences(String path, StringBuilder sequences)
        {
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    appendSequenceData(sw, sequences);
                }
            }
            else
            {

                // This text is always added, making the file longer over time 
                // if it is not deleted. 
                using (StreamWriter sw = File.AppendText(path))
                {
                    appendSequenceData(sw, sequences);
                }
            }
        }
        private void appendSequenceData(StreamWriter sw, StringBuilder sequences)
        {
            sw.WriteLine(">" + bsynid + " " + domain + " " + bb);
            sw.WriteLine(sequences);
            sw.WriteLine("\n");
        }

        private void panel2_Scroll(object sender, ScrollEventArgs e)
        {
          //  barChart.Invalidate();
            panel2.Invalidate();
        }

       

      
        //public IEnumerable<Rectangle> GetRectangles(Graphics g1)
        //{
        //    //int X = 3;
        //    //int left = X;
        //    //String word = "sadas";
        //    //foreach (char ch in word)
        //    //{

        //    //    //actual width is the (width of XX) - (width of X) to ignore padding
        //    //    var size = g1.MeasureString("" + ch, Font);
        //    //    var size2 = g1.MeasureString("" + ch + ch, Font);

        //    //    using (Bitmap b = new Bitmap((int)size.Width + 2, (int)size.Height + 2))
        //    //    using (Graphics g = Graphics.FromImage(b))
        //    //    {
        //    //        g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);
        //    //        g.TextRenderingHint = g1.TextRenderingHint;
        //    //        g.DrawString("" + ch, Font, Brushes.Black, 0, 0);
        //    //        int top = -1;
        //    //        int bottom = -1;

        //    //        //find the top row
        //    //        for (int y = 0; top < 0 && y < (int)size.Height - 1; y++)
        //    //        {
        //    //            for (int x = 0; x < (int)size.Width; x++)
        //    //            {
        //    //                if (b.GetPixel(x, y).B < 2)
        //    //                {
        //    //                    top = y;
        //    //                }
        //    //            }
        //    //        }

        //    //        //find the bottom row
        //    //        for (int y = (int)(size.Height - 1); bottom < 0 && y > 1; y--)
        //    //        {
        //    //            for (int x = 0; x < (int)size.Width - 1; x++)
        //    //            {
        //    //                if (b.GetPixel(x, y).B < 2)
        //    //                {
        //    //                    bottom = y;
        //    //                }
        //    //            }
        //    //        }
        //    //        yield return new Rectangle(left, Y + top, (int)(size2.Width - size.Width), bottom - top);
        //    //    }
        //    //    left += (int)(size2.Width - size.Width);
        //    }

        //}
    }
}
