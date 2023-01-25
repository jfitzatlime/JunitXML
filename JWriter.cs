using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using System.Runtime;

namespace JunitXML
{
    internal class JWriter
    {
        private XmlWriter xm;
        private XmlWriterSettings xsettings;
        private string outputfolder = @"C:\Users\jfitz\Desktop\TestOutput\";
        private string defaultclass = "cls";
        private string testtime = "0.0";
        private string assertcount = "0.0";

        public XmlWriter Xm
        {
            get { return xm; }
            set { xm = value; }
        }

        public string Outputfolder
        {
            get { return outputfolder; }
            set { outputfolder = value; }
        }

        public JWriter(string rootFolder, string subFolder)
        {
            outputfolder = @"C:\Users\jfitz\Desktop\TestOutput\";
            defaultclass = "cls";

            Directory.SetCurrentDirectory(outputfolder);

            xsettings = new XmlWriterSettings();
            xsettings.Indent = true;

            xm = XmlWriter.Create(Path.Combine(outputfolder, "JUnit_For_TQ_Import" + ".Xml"), xsettings);
            
            xm.WriteStartDocument();
            xm.WriteStartElement("testsuites");
            xm.WriteAttributeString("name", rootFolder);
            xm.WriteStartElement("testsuite");
            xm.WriteAttributeString("name", subFolder);
            xm.WriteAttributeString("errors", "0");
            xm.WriteAttributeString("failures", "0");
            xm.WriteAttributeString("id", "0");
            xm.WriteAttributeString("tests", "0");
        }

        // destructor
        public void Close()
        {
            // 02 Write the testsuite element
            xm.WriteEndElement();
            // 01 Write the root element
            xm.WriteEndElement();
            // Close the XML document
            xm.WriteEndDocument();
            // Close the XML writer
            xm.Close();
        }

        public void logTestResult(string strTestCaseName,
                        string strStatus,
                        string strMessage,
                        string strExceptionMessage
        )
        {
            xm.WriteStartElement("testcase");
            xm.WriteAttributeString("classname", defaultclass);
            xm.WriteAttributeString("name", strTestCaseName);
            xm.WriteAttributeString("time", testtime);
            xm.WriteAttributeString("assertions", assertcount);
            xm.WriteAttributeString("status", strStatus);

            if (strStatus.StartsWith("fail"))
            {
                xm.WriteStartElement("failure");
                xm.WriteAttributeString("message", strMessage);
                xm.WriteString(strExceptionMessage);
                xm.WriteEndElement(); // failure
            }

            if (strStatus.StartsWith("error"))
            {
                xm.WriteStartElement("error");
                xm.WriteAttributeString("message", strMessage);
                xm.WriteString(strExceptionMessage);
                xm.WriteEndElement(); // error
            }

            xm.WriteEndElement(); // testcase
        }
        public void logTestResult(string strTestCaseName,
                        string strStatus,
                        string strMessage
        )
        {
            logTestResult (strTestCaseName, strStatus, strMessage, "");
        }

    }
}
