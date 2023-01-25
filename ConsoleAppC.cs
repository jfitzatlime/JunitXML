namespace ConsoleApp1
{
    class Program
    {
        static double[] dozeTime = new double[] { 0.15, 1.01, 2.02, 2.1, 2.2, 2.3, 2.4, 2.5, 90, 120, 360, 600, 3600 };
        public static void Main(string[] args)
        {
            #region 001 - Declares
            
            string myURL = "https://di-test.lime-energy.app";
            string myUserName = "jfitz";
            string myPassword = "YH7uuJJ#e4";
            string myNewBeforeECM01;
            // string myNewBeforeECM02;
            bool myAuditAdditionalInfoNeeded = false;

            myProgram prg = new myProgram("SNTC");

            mySleep(dozeTime[1], "Starting");

            DateTime myGlobDate = new DateTime();
            myGlobDate = DateTime.Now;
            string date_str = myGlobDate.ToString("MM/dd/yyyy HH:mm:ss");

            myRnd thisRnd = new myRnd();
            #endregion

            #region 002 - Startup

            IWebDriver driver = new ChromeDriver(@"C:\ChromeDriver");
            System.TimeSpan myT = new TimeSpan(0,0,600);    
            driver.Manage().Timeouts().ImplicitWait = myT;
            mySleep(dozeTime[3], "Sleep to allow window drags.");

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            driver.Navigate().GoToUrl(myURL);
            // driver.Manage().Window.Maximize();

            IWebElement txtLoginName = driver.FindElement(By.Id("okta-signin-username"));
            IWebElement txtLoginPWD = driver.FindElement(By.Id("okta-signin-password"));
            IWebElement btnSignIn = driver.FindElement(By.Id("okta-signin-submit"));
            txtLoginName.SendKeys(myUserName);
            mySleep(dozeTime[0]);
            txtLoginPWD.SendKeys(myPassword);
            mySleep(dozeTime[0], "Username and Password keys sent.");
            btnSignIn.Click();

            mySleep(dozeTime[0], "Get the 2 Factor Code.");

            var otpKeyStr = "5IOQ4QMMB2OVQ64P";   // my key, obtained when turning on 2FA
            var otpKeyBytes = Base32Encoding.ToBytes(otpKeyStr);
            var otp = new Totp(otpKeyBytes);
            var twoFactorCode = otp.ComputeTotp();
            mySleep(dozeTime[0], "2 Factor Code - computed: " + twoFactorCode);

            /* find the 2 factor txt box and send keys to it */
            IWebElement txt2FactorCode = driver.FindElement(By.Id("input71"));
            txt2FactorCode.SendKeys(twoFactorCode);
            mySleep(dozeTime[0], "2 Factor Code " + twoFactorCode + " keys sent to textbox.");

            /* find the Verify button and click it. */
            IWebElement btnFactorVerify = driver.FindElement(By.ClassName("button-primary"));
            mySleep(dozeTime[0], "Verify button found;  do Click");
            mySleep(dozeTime[0]);
            btnFactorVerify.Click();

            mySleep(dozeTime[1], "Find the Select Program cbo and select program " + prg.programName);

            IWebElement cboSelectProgram = driver.FindElement(By.Id("SelectedProgramId"));
            SelectElement selectedProgram = new SelectElement(cboSelectProgram);
            selectedProgram.SelectByText(prg.programName);                          
            #endregion

            #region 003NEW - Find Application

            mySleep(dozeTime[2], "Click menu-APPLICATIONs");
            IWebElement btnApplications = driver.FindElement(By.Id("hlCustomers"));
            btnApplications.Click();

            mySleep(dozeTime[0]);
            IWebElement txtAppSearch = driver.FindElement(By.Id("query"));
            txtAppSearch.SendKeys(prg.applicationId);           

            mySleep(dozeTime[0], "Find search by Application ID button.");
            IWebElement btnAppSearch = driver.FindElement(By.Id("doSearchByApplicationId"));
            btnAppSearch.Click();
            mySleep(dozeTime[3], "... now wait some for Application search");

            driver.FindElement(By.CssSelector("td:nth-child(2)")).Click();
            driver.FindElement(By.Id("ButtonEdit")).Click();
        
            var myElm = driver.FindElement(By.LinkText("Available Applications"));
            Actions builder = new Actions(driver);
            builder.MoveToElement(myElm).Perform();
         
            driver.FindElement(By.LinkText("Available Applications")).Click();
            mySleep(dozeTime[3], "Available Applications - Clicked.");

            // driver.Manage().Window.Maximize();

            var myElm2 = driver.FindElement(By.TagName("body"));
            Actions builder2 = new Actions(driver);
            mySleep(dozeTime[3], "Give TIME for element to be MovedTo....PreScroll");

            js.ExecuteScript("window.scrollBy(0,document.body.scrollHeight)", "");

            mySleep(dozeTime[3], "Give TIME for element to be MovedTo....POSTScroll");

            // builder2.MoveToElement(myElm2, 0, 0).Perform();
            
            {
                var myElm3 = driver.FindElement(By.Id("appAttach"));
                Actions builder3 = new Actions(driver);
                builder3.MoveToElement(myElm3).Perform();
            }
            
            {
                var myElm4 = driver.FindElement(By.TagName("body"));
                Actions builder4 = new Actions(driver);
                //builder4.MoveToElement(myElm4, 0, 0).Perform();
            }
            
            driver.FindElement(By.CssSelector(".t-edit")).Click();
            driver.FindElement(By.Id("SupplyUtilityAccountNumbers")).Click();
            driver.FindElement(By.Id("SupplyUtilityAccountNumbers")).SendKeys("supQA");
            driver.FindElement(By.Id("DoingBusinessAs")).Click();
            driver.FindElement(By.Id("DoingBusinessAs")).Clear();
            driver.FindElement(By.Id("DoingBusinessAs")).SendKeys("dbasQA");
            driver.FindElement(By.Id("ContactPerson")).Click();
            driver.FindElement(By.Id("ContactPerson")).Clear();
            driver.FindElement(By.Id("ContactPerson")).SendKeys("conperQA");
            driver.FindElement(By.Id("Title")).Click();
            driver.FindElement(By.Id("Title")).Clear();
            driver.FindElement(By.Id("Title")).SendKeys("President");
            driver.FindElement(By.CssSelector(".t-edit-form-container")).Click();
            driver.FindElement(By.Id("ContactPhoneNumber")).Clear();
            driver.FindElement(By.Id("ContactPhoneNumber")).SendKeys("1112223333");
            driver.FindElement(By.Id("AlternatePhoneNumber")).Clear();
            driver.FindElement(By.Id("AlternatePhoneNumber")).SendKeys("4445557777");
            driver.FindElement(By.Id("appmodelfacility-fieldset")).Click();
            driver.FindElement(By.Id("EmailAddress")).Clear();
            driver.FindElement(By.Id("EmailAddress")).SendKeys("qatesttest708@gmail.com");
            driver.FindElement(By.Id("BillingAddress1")).Clear();
            driver.FindElement(By.Id("BillingAddress1")).SendKeys("QA Street");
            driver.FindElement(By.Id("ServiceAddress1")).Clear();
            driver.FindElement(By.Id("ServiceAddress1")).SendKeys("QA Street");

            mySleep(dozeTime[3], "Button Application Update - Click...");
            driver.FindElement(By.Id("btnApplicationUpdate")).Click();            
            mySleep(dozeTime[3], "Button Application Update - Clicked.  Now scroll down... then click Save.");


            js.ExecuteScript("window.scrollTo(0,309)");
            driver.FindElement(By.Name("btnSave")).Click();

            mySleep(dozeTime[3], "Saved.");

            driver.FindElement(By.Id("addAuditLink")).Click();
            /* driver.FindElement(By.Id("Audit_Description")).Click();
            driver.FindElement(By.Id("Audit_Description")).Clear();
            driver.FindElement(By.Id("Audit_Description")).SendKeys("--- Dec1 ---");
            mySleep(dozeTime[3], "Keys sent."); */

            driver.FindElement(By.Id("Audit_DateProposed")).Click();
            driver.FindElement(By.Id("Audit_DateProposed")).Clear();
            // 24 | type | id=Audit_DateProposed | 12/12/2022
            driver.FindElement(By.Id("Audit_DateProposed")).SendKeys("12/05/2022");
            // 25 | click | id=Audit_EndDateTime | 
            driver.FindElement(By.Id("Audit_EndDateTime")).Clear();
            driver.FindElement(By.Id("Audit_EndDateTime")).Click();
            // 26 | type | id=Audit_EndDateTime | 12/31/2022
            driver.FindElement(By.Id("Audit_EndDateTime")).SendKeys("12/31/2022");

            mySleep(dozeTime[3], "Audit Proposed and End Date - 2 Dates typed in.");

            driver.FindElement(By.Id("Audit_Category")).Click();
            {
                var dropdown = driver.FindElement(By.Id("Audit_Category"));
                dropdown.FindElement(By.XPath("//option[. = 'Lighting']")).Click();
            }
            driver.FindElement(By.Id("Audit_Notes")).Clear();
            driver.FindElement(By.Id("Audit_Notes")).SendKeys("qa notes 333pm");
            driver.FindElement(By.Id("Audit_BusinessTypeId")).Click();
            {
                var dropdown = driver.FindElement(By.Id("Audit_BusinessTypeId"));
                dropdown.FindElement(By.XPath("//option[. = 'Agricultural, Forestry, Fishing: FISHING HUNTING & TRAPPING']")).Click();
            }

            mySleep(dozeTime[3], "OpHrs.");

            driver.FindElement(By.Id("Audit_OperatingHours")).Click();
            // driver.FindElement(By.CssSelector(".fieldsetPadding > div > div:nth-child(5)")).Click();
            driver.FindElement(By.Id("Audit_OperatingHours")).Clear();
            
            {
                var element = driver.FindElement(By.Id("Audit_OperatingHours"));
                Actions builder5 = new Actions(driver);
                builder5.DoubleClick(element).Perform();
            }

            driver.FindElement(By.Id("Audit_OperatingHours")).SendKeys("55");

            mySleep(dozeTime[5], "Keys sent -- 55.");

            driver.FindElement(By.Id("auditButtonSave")).Click();

            mySleep(7.33, "Save Clicked");

            driver.FindElement(By.LinkText("Floors And Rooms")).Click();
            mySleep(7.33, "Floors and Rooms,  Clicked");
            driver.FindElement(By.CssSelector("#FloorsGrid > .t-toolbar .t-icon")).Click();
            mySleep(5.33, "FloorsGrid .t-toolbar .t-icon)).Click() done...  Send floorname next.");
            driver.FindElement(By.Id("Label")).Click();
            driver.FindElement(By.Id("Label")).Clear();
            driver.FindElement(By.Id("Label")).SendKeys("Floor Dec5noon");
            mySleep(0.33);
            driver.FindElement(By.Id("Height")).Click();
            driver.FindElement(By.Id("Height")).Clear();
            driver.FindElement(By.Id("Height")).SendKeys("8");

            mySleep(dozeTime[4], "Height 8 sent");

            driver.FindElement(By.CssSelector(".t-insert")).Click();
            mySleep(dozeTime[6], "A");
            // #FloorsGrid > div.t-grid-content > table > tbody > tr > td:nth-child(2)
            driver.FindElement(By.CssSelector("#FloorsGrid > div.t-grid-content > table > tbody > tr > td:nth-child(2)")).Click();
            // driver.FindElement(By.CssSelector(".t-state-hover > td:nth-child(2)")).Click();
            mySleep(dozeTime[6], "B");
            driver.FindElement(By.CssSelector("#RoomsGrid > .t-toolbar .t-icon")).Click();
            mySleep(dozeTime[6], "C");
            driver.FindElement(By.Id("Label")).Click();
            driver.FindElement(By.Id("Label")).Clear();
            mySleep(dozeTime[3], "D");
            driver.FindElement(By.Id("Label")).SendKeys("Room 6789");
            driver.FindElement(By.Id("cobrand")).Click();
            driver.FindElement(By.Id("Height")).SendKeys("9");
            driver.FindElement(By.Id("RoomSpaceType")).Click();
            {
                var dropdown = driver.FindElement(By.Id("RoomSpaceType"));
                dropdown.FindElement(By.XPath("//option[. = 'Default Facility Hours']")).Click();
            }
            mySleep(dozeTime[3], "EE");
            driver.FindElement(By.CssSelector(".t-insert")).Click();
            mySleep(dozeTime[3], "FFF");
            driver.FindElement(By.LinkText("Line Items")).Click();

            mySleep(dozeTime[3], "Line items tab clicked.");

            js.ExecuteScript("window.scrollTo(0,12)");

            mySleep(dozeTime[3], "Scrolled down.");

            driver.FindElement(By.Id("add_new_lineitem")).Click();

            mySleep(dozeTime[3], "Add Line Item Clicked.");

            driver.FindElement(By.CssSelector(".panelbefore .seeker > strong")).Click();
            driver.FindElement(By.Id("code")).Click();
            driver.FindElement(By.Id("code")).SendKeys("2444");
            driver.FindElement(By.Id("apply")).Click();
            driver.FindElement(By.CssSelector(".data:nth-child(2) > td:nth-child(3)")).Click();
            //driver.FindElement(By.CssSelector(".data:nth-child(2) > td:nth-child(3)")).Click();
            {
                var element = driver.FindElement(By.CssSelector(".data:nth-child(2) > td:nth-child(3)"));
                Actions builder6 = new Actions(driver);
                builder6.DoubleClick(element).Perform();
            }

            driver.FindElement(By.Name("AuditBeforeConditionQuantity")).Click();
            driver.FindElement(By.Name("AuditBeforeConditionQuantity")).Clear();
            driver.FindElement(By.Name("AuditBeforeConditionQuantity")).SendKeys("19");
            driver.FindElement(By.Name("AuditBeforeConditionHeight")).Click();
            driver.FindElement(By.Name("AuditBeforeConditionHeight")).Clear();
            driver.FindElement(By.Name("AuditBeforeConditionHeight")).SendKeys("11");
            driver.FindElement(By.Name("AuditAfterConditionQuantity")).Click();
            driver.FindElement(By.Name("AuditAfterConditionQuantity")).Clear();
            driver.FindElement(By.Name("AuditAfterConditionQuantity")).SendKeys("19");
            driver.FindElement(By.Name("AuditAfterConditionHeight")).Click();
            driver.FindElement(By.Name("AuditAfterConditionHeight")).Clear();
            driver.FindElement(By.Name("AuditAfterConditionHeight")).SendKeys("11");
            driver.FindElement(By.CssSelector(".panelafter .seeker > strong")).Click();
            driver.FindElement(By.CssSelector(".data:nth-child(1) > td:nth-child(3)")).Click();
            driver.FindElement(By.CssSelector(".data:nth-child(1) > td:nth-child(3)")).Click();
            {
                var element = driver.FindElement(By.CssSelector(".data:nth-child(1) > td:nth-child(3)"));
                Actions builder7 = new Actions(driver);
                builder7.DoubleClick(element).Perform();
            }
            driver.FindElement(By.CssSelector(".current > ul .fg-button:nth-child(1)")).Click();
            {
                var element = driver.FindElement(By.CssSelector(".ui-corner-left:nth-child(1)"));
                Actions builder8 = new Actions(driver);
                builder8.MoveToElement(element).Perform();
            }

            // try the copy
            /*
            driver.FindElement(By.Id("copyButton")).Click();
            mySleep(0.33);
            driver.FindElement(By.CssSelector(".ui-state-focus > .ui-button-text")).Click();
            mySleep(0.33);
            driver.FindElement(By.CssSelector(".t-alt #editButton")).Click();
            mySleep(0.33);
            js.ExecuteScript("window.scrollTo(0,665)");
            mySleep(0.33);
            driver.FindElement(By.CssSelector(".t-alt .current > .panelbefore > table:nth-child(2) td:nth-child(1)")).Click();
            mySleep(0.33);
            driver.FindElement(By.Name("AuditBeforeConditionQuantity")).Clear();
            mySleep(0.33);
            driver.FindElement(By.Name("AuditBeforeConditionQuantity")).SendKeys("37");
            mySleep(0.33);
            driver.FindElement(By.CssSelector(".t-alt .current > .panelafter > table:nth-child(2) td:nth-child(1)")).Click();
            mySleep(0.33);
            driver.FindElement(By.Name("AuditAfterConditionQuantity")).Clear();
            mySleep(0.33);
            driver.FindElement(By.Name("AuditAfterConditionQuantity")).SendKeys("37");
            mySleep(0.33);
            driver.FindElement(By.CssSelector("ul .ui-corner-all:nth-child(1)")).Click();
            */

            mySleep(0.33);
            js.ExecuteScript("window.scrollTo(0,665)");
            mySleep(0.33);
            mySleep(dozeTime[3], "Click AuditButtonSave");
            driver.FindElement(By.Id("auditButtonSave")).Click();
            mySleep(dozeTime[3], "clicked, bye");
            #endregion

                #region 003 - Find Audit - Back to the Orig code...
                mySleep(dozeTime[0], "Click menu-Audits");
                IWebElement btnAudits = driver.FindElement(By.Id("hlAudits"));
                btnAudits.Click();

                mySleep(dozeTime[0]);
                IWebElement txtSearch = driver.FindElement(By.Id("query"));
                txtSearch.SendKeys(prg.applicationId);

                mySleep(dozeTime[0], "Find search by Application ID button.");
                IWebElement btnSearch = driver.FindElement(By.Id("doSearchByApplicationId"));
                btnSearch.Click();
                mySleep(dozeTime[2], "... now wait some for Application search");

                mySleep(dozeTime[1], "Find Documentation button in the grid , to click for audit open...");
                IWebElement btnDoc = driver.FindElement(By.XPath("//input[@value='Documentation']"));
                mySleep(dozeTime[2], "... found, Click Documentation button.");
                btnDoc.Click();

                // here the Audits page will load and takes about 60 seconds.
                // revisit to implement 'waitforpageload' call
                mySleep(dozeTime[0], "Find the Details tab.");

                // Details tab
                IWebElement tabDetail = driver.FindElement(By.XPath("//*[text()='Details']"));
                mySleep(dozeTime[0], "Click the Details tab.");
                tabDetail.Click();
                mySleep(dozeTime[0]);

                DateTime myDate = new DateTime();
                IWebElement txtAuditNotes = driver.FindElement(By.Id("Audit_Notes"));
                myDate = DateTime.Now;
                string dt_str; //  = myDate.ToLongDateString();                
                dt_str = myDate.ToString("MM/dd/yyyy HH:mm:ss");
                txtAuditNotes.SendKeys(" -- QA Tested at > " + dt_str + " <" + Keys.Enter);

                #endregion

                #region 004 - Edit LineItem calls
                //=================================

                mySleep(dozeTime[1]);
                IWebElement tabLineItems = driver.FindElement(By.XPath("//*[text()='Line Items']"));
                tabLineItems.Click();

                // edit line item
                myNewBeforeECM01 = thisRnd.myRndBeforeCode(prg.programName);
                mySleep(dozeTime[1], "Try 1 - edit line item to " + myNewBeforeECM01);
                myEditLineItem(driver, myNewBeforeECM01, myNewBeforeECM01);
                mySleep(dozeTime[3], "Done Edit Line Item try 1.");

            /*
            myNewBeforeECM02 = thisRnd.myRndBeforeCode(prg.programName);
            if (myNewBeforeECM01 != myNewBeforeECM02)
            {
                mySleep(dozeTime[1], "Try 2 - edit line item to " + myNewBeforeECM02);
                myEditLineItem(driver, myNewBeforeECM01, myNewBeforeECM01);
                mySleep(dozeTime[1], "Done Edit Line Item try 2.");
            }
            */
            //=================================
            #endregion

                #region 005 - Additional Info and Save, working right up to create project

                if (myAuditAdditionalInfoNeeded)
                {

                    mySleep(dozeTime[1], "Find btnAddnInfo");
                    // IWebElement btnAddnInfo = driver.FindElement(By.XPath("//input[@value='Additional Info']"));
                    IWebElement btnAddnInfo = driver.FindElement(By.Id("AdditionalInfoTab"));
                    mySleep(dozeTime[1], "Click btnAddnInfo");
                    btnAddnInfo.Click();

                    mySleep(dozeTime[1], "Find the Building Type cbo");
                    IWebElement cboBT = driver.FindElement(By.Id("Audit_Extensions_lght_buildingType"));
                    SelectElement selectedBT = new SelectElement(cboBT);
                    selectedBT.SelectByText("Restaurants");

                    mySleep(dozeTime[1], "Find the HVAC Type cbo");
                    IWebElement cboHV = driver.FindElement(By.Id("Audit_Extensions_lght_hvacType"));
                    SelectElement selectedHV = new SelectElement(cboHV);
                    selectedHV.SelectByText("Heat Pump");

                }

                mySleep(dozeTime[0], "Click the Details tab.");
                tabDetail.Click();
                mySleep(dozeTime[2], "Clicked now wait a bit.");

                mySleep(dozeTime[1], "Click Audit Save button");
                IWebElement btnAuditSave = driver.FindElement(By.Id("auditButtonSave"));
                btnAuditSave.Click();

                //Store the ID of the original window
                string DIwindow = driver.CurrentWindowHandle;

                mySleep(dozeTime[1], "Proposal");
                IWebElement btnAuditProposal = driver.FindElement(By.Id("btnGenerateProposalId"));
                btnAuditProposal.Click();
                mySleep(dozeTime[2], "short wait following click Proposal");

                //Click the link which opens in a new window
                //driver.FindElement(By.LinkText("closer-stage052021.lime-energy.app/")).Click();

                // IWebElement btnWhite = driver.FindElement(By.Id("btnGenerateProposalId"));

                //Wait for the new window or tab
                WebDriverWait waitWin = new WebDriverWait(driver, new TimeSpan(0, 0, 600));
                waitWin.Until(wd => wd.WindowHandles.Count == 2);
                //Loop through until we find a new window handle
                foreach (string winstr in driver.WindowHandles)
                {
                    mySleep(dozeTime[2], "Window loop ; " + winstr);
                    if (DIwindow != winstr)
                    {
                        driver.SwitchTo().Window(winstr);
                        break;
                    }
                }

                string CLOSERwindow = driver.CurrentWindowHandle;
                if (DIwindow == CLOSERwindow)
                { mySleep(dozeTime[1], "2 Windows are the same " + DIwindow); }
                else
                { mySleep(dozeTime[1], "2 Windows are the DIF  " + DIwindow + " <> " + CLOSERwindow); }

                mySleep(dozeTime[2], "Find and click the white button.");
                IWebElement btnSignInButton = driver.FindElement(By.ClassName("signInButtonContent"));
                btnSignInButton.Click();
                mySleep(dozeTime[2], "Clicked.");

                //Wait for the new tab to finish loading content
                //waitWin.Until(wd => wd.Title == "Closer");

                #endregion

                #region NEW - Project Create - May 4

                mySleep(dozeTime[2], "Switch back to DI window...");
                driver.SwitchTo().Window(DIwindow);

                mySleep(dozeTime[1], "Find and click Create Project");
                IWebElement btnCreateProject = driver.FindElement(By.Id("btnCreateProjectId"));
                btnCreateProject.Click();
                mySleep(dozeTime[1], "Create Project - Clicked");


                mySleep(dozeTime[3], "*Find and click YES for Project Create");
                //IWebElement btnYESdoCreateProject = driver.FindElement(By.XPath("//*[text()='Yes']"));
                IWebElement btnYESdoCreateProject = driver.FindElement(By.ClassName("ui-button-text"));
                btnYESdoCreateProject.Click();
                mySleep(dozeTime[3], "*Yes - Clicked");

                mySleep(dozeTime[3], "*wait before looking for Hubspot HERE");
                IWebElement btnHERE2 = driver.FindElement(By.CssSelector("#userHubNotificationQueue_NOTIFICATIONS > div:nth-child(3) > a"));

                // driver.FindElement(By.Name("HERE"));
                mySleep(dozeTime[3], "* HERE - found - click it.");
                btnHERE2.Click();

                #endregion

                #region ProjectPage

                //Wait for the new window or tab
                WebDriverWait waitWin2 = new WebDriverWait(driver, new TimeSpan(0, 0, 600));
                waitWin2.Until(wd => wd.WindowHandles.Count > 2);
                //Loop through until we find a new window handle
                foreach (string winstr2 in driver.WindowHandles)
                {
                    mySleep(dozeTime[2], "*Window loop ; " + winstr2);
                    if ((DIwindow != winstr2) && (CLOSERwindow != winstr2))
                    {
                        driver.SwitchTo().Window(winstr2);
                        break;
                    }
                }

                string PROJwindow = driver.CurrentWindowHandle;
                if (DIwindow == PROJwindow)
                { mySleep(dozeTime[1], "*2 Windows are the same " + DIwindow); }
                else
                { mySleep(dozeTime[1], "*2 Windows are the DIF (PROJ) " + DIwindow + " <> " + PROJwindow); }


                //

                IWebElement txtProjNotes = driver.FindElement(By.Id("Project_Notes"));
                myDate = DateTime.Now;
                dt_str = myDate.ToString("MM/dd/yyyy HH:mm:ss");
                txtProjNotes.SendKeys(" -- QA Automated Test -- Project created at > " + dt_str + " <" + Keys.Enter);

                mySleep(11, "*Find Project Documentation tab, after wait 22 ... ");
                // IWebElement btnAddnInfo = driver.FindElement(By.XPath("//input[@value='Additional Info']"));
                IWebElement btnProjDoc = driver.FindElement(By.XPath("//*[text()='Documentation']"));
                mySleep(dozeTime[1], "*Click Project Documentation tab");
                btnProjDoc.Click();

                mySleep(11, "*Documentation tab Clicked - now wait 22 before find Labor PO in grid");
                mySleep(1, "*Done wait now find in grid...");

                IWebElement poLaborGrdCell = driver.FindElement(By.XPath("//*[text()[contains(.,'Purchase Order - Labor')]]"));
                poLaborGrdCell.Click();
                mySleep(5, "* grid clicked for Labor PO");

                IWebElement btnIssueDoc = driver.FindElement(By.Id("btnRunReport"));
                btnIssueDoc.Click();
                mySleep(5, "* Issue Document  -- clicked");

                mySleep(15, "* Wait before clicking to next doc...");
                mySleep(0.21, "* Now find grid cell and click...");
                IWebElement poMaterialGrdCell = driver.FindElement(By.XPath("//*[text()[contains(.,'Purchase Order - Material')]]"));
                poMaterialGrdCell.Click();
                mySleep(5, "* grid clicked for Material PO");

                // btnIssueDoc = driver.FindElement(By.Id("btnRunReport"));
                btnIssueDoc.Click();
                mySleep(5, "* Issue Document  -- clicked");

                mySleep(15, "* Wait before clicking to next doc...");
                mySleep(0.21, "* Now find grid cell and click...");
                IWebElement pcfGrdCell = driver.FindElement(By.XPath("//*[text()[contains(.,'Project Completion Form')]]"));
                pcfGrdCell.Click();
                mySleep(5, "* grid clicked for PCF");

                // btnIssueDoc = driver.FindElement(By.Id("btnRunReport"));
                btnIssueDoc.Click();
                mySleep(5, "* Issue Document  -- clicked");

                mySleep(15, "* Wait before clicking to next doc...");
                mySleep(0.21, "* Now find grid cell and click...");
                IWebElement coaGrdCell = driver.FindElement(By.XPath("//*[text()[contains(.,'Project Change Order Authorization')]]"));
                coaGrdCell.Click();
                mySleep(5, "* grid clicked for COA");

                // btnIssueDoc = driver.FindElement(By.Id("btnRunReport"));
                btnIssueDoc.Click();
                mySleep(5, "* Issue Document  -- clicked");

                mySleep(15, "* Wait before clicking to next doc...");
                mySleep(0.21, "* Now find grid cell and click...");
                IWebElement d179GrdCell = driver.FindElement(By.XPath("//*[text()[contains(.,'179D Letter')]]"));
                d179GrdCell.Click();
                mySleep(5, "* grid clicked for COA");

                // btnIssueDoc = driver.FindElement(By.Id("btnRunReport"));
                btnIssueDoc.Click();
                mySleep(5, "* Issue Document  -- clicked");

                mySleep(15, "* Wait before clicking to next doc...");
                mySleep(0.21, "* Now find grid cell and click...");
                IWebElement scopeGrdCell = driver.FindElement(By.XPath("//*[text()[contains(.,'Scope of Work (specific by scope)')]]"));
                scopeGrdCell.Click();
                mySleep(5, "* grid clicked for COA");

                // btnIssueDoc = driver.FindElement(By.Id("btnRunReport"));
                btnIssueDoc.Click();
                mySleep(5, "* Issue Document  -- clicked");

                // Scope of Work (specific by scope)
                #endregion

            Console.WriteLine("Test Completed");
        }

        #region 006 - Sleeps
        public static void mySleep(double nSecs)
        { System.Threading.Thread.Sleep(Convert.ToInt32(nSecs * 1000)); }
        public static void mySleep(double nSecs, string sMsg)
        {
            DateTime myDate = new DateTime();
            string s1 = sMsg.Substring(0, 1);
            myDate = DateTime.Now;
            string date_str = myDate.ToString("MM/dd/yyyy HH:mm:ss");
            // if (s1 == "*")
            //    Console.WriteLine("Start sleep for " + nSecs.ToString() + " at " + date_str + " ... "); 
            System.Threading.Thread.Sleep(Convert.ToInt32(nSecs * 1000));
            myDate = DateTime.Now;
            date_str = myDate.ToString("MM/dd/yyyy HH:mm:ss");
            // if (s1 == "*")
            //Console.WriteLine("... sleep done for --> " + sMsg + " <-- " + " at " + date_str + " ... ");
            Console.WriteLine(sMsg + " ; " + " at - " + date_str + " for " + nSecs.ToString() + " seconds.");
            //Console.WriteLine("");
        }
        #endregion

        #region 007 - Edit LineItem Def
        private static void myEditLineItem(IWebDriver d, string strNewCode, string strFiltStr)
        {
            System.TimeSpan myT2 = new TimeSpan(0, 0, 600);
            WebDriverWait myWebWait = new WebDriverWait(d, myT2);

            mySleep(dozeTime[1], "myEditLineItem -- Find and click Edit (lineitem).  wait.until ExpectedConditions.ElementToBeClickable");
            IWebElement btnEditLineItem = d.FindElement(By.Id("editButton"));
            myWebWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(btnEditLineItem));

            mySleep(dozeTime[1], "Dialog cleared.  Edit Line Item button should now be clickable.");
            btnEditLineItem.Click();

            myRnd r2d2 = new myRnd();
            string newQ = r2d2.myRnd60().ToString();

            mySleep(dozeTime[1], "Set before quantity.");
            IWebElement txtBeforeQuantity = d.FindElement(By.CssSelector("input[name='AuditBeforeConditionQuantity']"));
            txtBeforeQuantity.Clear();
            txtBeforeQuantity.SendKeys(newQ);
            
            mySleep(dozeTime[1], "Set after quantity.");
            IWebElement txtAfterQuantity = d.FindElement(By.Name("AuditAfterConditionQuantity"));
            txtAfterQuantity.Clear();
            txtAfterQuantity.SendKeys(newQ);

            mySleep(dozeTime[1], "Find Master Code (link/btn) and click it.");
            // find 'Master Code' and click it to bring up the Master Code selector dialog
            IWebElement btnMasterCodeBefore = d.FindElement(By.XPath("//*[text()='Master Code']"));
            myWebWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(btnMasterCodeBefore));
            btnMasterCodeBefore.Click();

            mySleep(dozeTime[1], "Find ECM " + strNewCode);
            string myECM = strNewCode;
            IWebElement myFilt = d.FindElement(By.Id("code"));
            myFilt.Clear();
            myFilt.SendKeys(strFiltStr);
            mySleep(dozeTime[1], "Next, click Apply btn.");

            IWebElement btnApplyFilt = d.FindElement(By.Id("apply"));
            btnApplyFilt.Click();
            mySleep(dozeTime[1], "Apply Clicked.");

            myFilt.Clear();
            mySleep(dozeTime[1], "Filter Cleared");

            IWebElement grdCellECMselected = null;
            string myECM2 = "//*[text()='" + myECM + "']";
            try
            {
                grdCellECMselected = d.FindElement(By.XPath(myECM2));
                mySleep(dozeTime[1], "Found the ECM text.  About to dbl-clickit.");
            }
            catch
            {
                mySleep(11, "NOT - Found the ECM text. Catch...");
            }

            Actions myAct = new Actions(d);
            myAct.DoubleClick(grdCellECMselected).Perform();

            mySleep(dozeTime[1], "DoubleClick.Performed.");

            // see if the dblclick did the desired action of closing the ECM selection dialog...

            /*
            IWebElement clsDlg = d.FindElement(By.Id("ecm_seeker_close"));
            clsDlg.Click();
            */


            /*
            try
            {
                IWebElement clsDlg = d.FindElement(By.Id("ecm_seeker_close"));
                clsDlg.Click();
            }
            catch
            {
                mySleep(dozeTime[1], "Catch 2");
            }
            */

            mySleep(dozeTime[1], "Dialog should be closed by NOW... Find Save (line item) button");

            IWebElement btnSaveLineItem = d.FindElement(By.CssSelector("button[class='fg-button ui-state-default  ui-priority-primary ui-corner-all']"));
            
            mySleep(dozeTime[1], "Save (Line Item) button found.  Next, setup a timespan 600 seconds to WaitForElementToBeClickable");
            
            System.TimeSpan myT3 = new TimeSpan(0, 0, 600);
            WebDriverWait myWebWait3 = new WebDriverWait(d, myT3);
            mySleep(dozeTime[3], "Now wait...");
            myWebWait3.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(btnSaveLineItem));
            mySleep(dozeTime[3], "... now IS clickable.");
            
            btnSaveLineItem.Click();
            mySleep(dozeTime[1], "Save Line Item Clicked.");
        }
        #endregion
    }
}


