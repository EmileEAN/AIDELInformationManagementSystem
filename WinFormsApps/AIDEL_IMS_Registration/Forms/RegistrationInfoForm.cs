using AppResources = AIDEL_IMS_Registration.Properties.Resources;
using EEANWorks.WinForms;
using EEANWorks.WinForms.UI;
using EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Data;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices.Autodiscover;
using System.Globalization;
using WFForm = System.Windows.Forms.Form;
using IronPdf;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Registration.Forms
{
    public partial class RegistrationInfoForm : System.Windows.Forms.Form
    {
        private readonly string m_username;
        private readonly string m_password;

        public RegistrationInfoForm()
        {
            DataContainer container = DataContainer.Instance;
            Dictionary<string, string> systemDataDictionary = container.SystemDataDictionary;
            m_username = systemDataDictionary["LastLoginUser"] + CoreValues.EmailExtension;
            m_password = systemDataDictionary["Password"];

            InitializeComponent();
        }


        private void RegistrationInfoForm_Load(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;

            InitializeMainViewTable();

            RefreshMainViewTable();
            dgv_main.ClearSelection();
            dgv_main.SelectionChanged += (_sender, _e) => dgv_main.ClearSelection();

            chckBx_hideEndedExams.Checked = true;
        }

        private bool m_isInitializingDgv = false;

        #region Component Events
        private void txtBx_filter_main_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter_Main();
        }

        private void dgv_main_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_isInitializingDgv)
                return;

            try
            {
                var dgv = (DataGridView)sender;

                int columnIndex = e.ColumnIndex;
                int rowIndex = e.RowIndex;
                var column = dgv_main.Columns[columnIndex];
                if (rowIndex >= 0) // If it is not the header cell
                {
                    if (column.Name == "ExamDate")
                    {
                        var cells = dgv.Rows[rowIndex].Cells;
                        var cell = cells[columnIndex];

                        int id = Convert.ToInt32((string)(cells["Id"].Value));
                        
                        DataContainer container = DataContainer.Instance;
                        var registrationInfo = container.RegistrationInfos_Internal.First(x => x.Id == id);

                        string stringValue = (string)(cell.Value);

                        DateTime newValue = (stringValue != CoreValues.ComboBox_DefaultString) ? DateTime.Parse(stringValue) : default;

                        Student examinee = container.StudentList.First(x => x.OrganizationEmail == registrationInfo.OrganizationEmail);
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to add " + examinee.Name.ToString() + " to the exam which will be conducted on " + registrationInfo.Exam.Date.ToString("MMMM dd, yyyy") + "?", "Caution!", MessageBoxButtons.YesNo);
                        if (dialogResult != DialogResult.Yes)
                        {
                            MessageBox.Show("Action Cancelled!");
                            var cmbBxCell = cell as DataGridViewComboBoxCell;
                            cmbBxCell.Value = CoreValues.ComboBox_DefaultString;
                            return;
                        }

                        int bookingNumber = 0;
                        {
                            NonInstitutionalExam_Internal exam = DataContainer.Instance.NonInstitutionalExam_InternalDictionary.Values.First(x => x.Type == registrationInfo.Exam.Type && x.Date == newValue);
                            List<RegistrationInfo_Internal> targetRegistrationInfos = container.RegistrationInfos_Internal.Where(x => x.Exam == exam
                                                                        && x.Status != eRegistrationStatus.Repeated
                                                                        && x.Status != eRegistrationStatus.Canceled)
                                                                .OrderBy(x => x.BookingNumber)
                                                                .ToList();
                            int registeredExamineesCount = targetRegistrationInfos.Count();
                            string bookingNumberString_examDatePortion = exam.Date.ToString("ddMMyy");

                            // Check each existing registration info for the exam; and if a booking number is missing, set that value as the booking number for the new registration info
                            for (int i = 1; i <= registeredExamineesCount; i++)
                            {
                                string bookingNumberString = (i.ToString("D2") + bookingNumberString_examDatePortion);
                                if (targetRegistrationInfos[i - 1].BookingNumber.ToString(CoreValues.BookingNumberStringFormat) != bookingNumberString)
                                {
                                    bookingNumber = Convert.ToInt32(bookingNumberString);
                                    break;
                                }
                            }
                            // If no booking number was missing, set the smallest booking number available
                            if (bookingNumber == 0)
                                bookingNumber = Convert.ToInt32((registeredExamineesCount + 1).ToString("D2") + bookingNumberString_examDatePortion);
                        }

                        DocumentLoader.EditRegistrationInfoInCSV(registrationInfo, newValue, bookingNumber);
                        container.SharePointConnectionManager.UploadFile(container.FilePath_RegistrationInfo_Internal);
                        container.ResetAllFileModificationStatus();

                        MessageBox.Show("Moved student from the waiting list to the selected examinee list!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to change date! " + ex.Message);
            }
        }

        private void dgv_main_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;

            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            var column = dgv.Columns[columnIndex];
            if (column is DataGridViewButtonColumn && rowIndex >= 0) // If it is a button column and it is not the header cell
            {
                var cells = dgv.Rows[rowIndex].Cells;

                int id = Convert.ToInt32((string)(cells["Id"].Value));

                DataContainer container = DataContainer.Instance;
                var registrationInfo = container.RegistrationInfos_Internal.First(x => x.Id == id);

                if (column.DataPropertyName == "Identification1")
                {
                    string documentURL = registrationInfo.Id1Url;
                    LoadImageAndDisplay_InternalRegistrationInfo(documentURL);
                }

                if (column.DataPropertyName == "Identification2")
                {
                    string documentURL = registrationInfo.Id2Url;
                    LoadImageAndDisplay_InternalRegistrationInfo(documentURL);
                }

                if (column.DataPropertyName == "RegistrationPdf")
                {
                    string documentURL = registrationInfo.RegistrationPdfUrl;
                    LoadPdfAndDisplay(documentURL);
                }    

                if (column.DataPropertyName == "Voucher" && (cells["Voucher"] as DataGridViewCustomButtonCell).Enabled)
                {
                    string documentURL = registrationInfo.VoucherUrl;
                    LoadImageAndDisplay(registrationInfo, documentURL);
                }

                if (column.DataPropertyName == "NextStep" && (cells["NextStep"] as DataGridViewCustomButtonCell).Enabled)
                {
                    string name = cells["Name"].Value.ToString();
                    ConfirmAndSendEmail(registrationInfo, name);
                    RefreshMainViewTable();
                }

                if (column.DataPropertyName == "Cancelation" && (cells["Cancelation"] as DataGridViewCustomButtonCell).Enabled)
                {
                    string name = cells["Name"].Value.ToString();
                    CancelRegistration(registrationInfo, name);
                    RefreshMainViewTable();
                }
            }
        }

        private void btn_addExam_Click(object sender, EventArgs e)
        {
            this.LogAndSwitchTo(new ExamScheduleForm());
        }

        private void btn_signOut_Click(object sender, EventArgs e)
        {
            DataContainer container = DataContainer.Instance;
            Dictionary<string, string> systemDataDictionary = container.SystemDataDictionary;

            systemDataDictionary["LastLoginUser"] = "";
            systemDataDictionary["Password"] = "";
            container.ResetAllFileModificationStatus(); // Changes to the two fields above are also applied to the csv save file through this method.

            this.SwitchTo(new LoginForm_NonInstitutionalExam());
        }
        #endregion 

        #region Component Initialization
        private void InitializeMainViewTable()
        {
            // Set columns
            if (dgv_main.ColumnCount == 0)
            {
                Font font = new Font("Palatino Linotype", 12F, GraphicsUnit.Pixel);

                var columns = dgv_main.Columns;
                var idColumn = new DataGridViewTextBoxColumn();
                idColumn.DataPropertyName = idColumn.Name = "Id";
                idColumn.HeaderText = "Id";
                idColumn.ReadOnly = true;
                columns.Add(idColumn);

                var statusColumn = new DataGridViewTextBoxColumn();
                statusColumn.DataPropertyName = statusColumn.Name = "Status";
                statusColumn.HeaderText = "Status";
                statusColumn.ReadOnly = true;
                columns.Add(statusColumn);

                var registrationDatecolumn = new DataGridViewTextBoxColumn();
                registrationDatecolumn.DataPropertyName = registrationDatecolumn.Name = "RegistrationDate";
                registrationDatecolumn.HeaderText = "Registration Date";
                registrationDatecolumn.ReadOnly = true;
                columns.Add(registrationDatecolumn);

                var examTypeColumn = new DataGridViewTextBoxColumn();
                examTypeColumn.DataPropertyName = examTypeColumn.Name = "Exam";
                examTypeColumn.HeaderText = "Exam";
                examTypeColumn.ReadOnly = true;
                columns.Add(examTypeColumn);

                var examDateColumn = new DataGridViewComboBoxColumn();
                examDateColumn.DataPropertyName = examDateColumn.Name = "ExamDate";
                examDateColumn.HeaderText = "Exam Date & Time";
                examDateColumn.ReadOnly = false;
                columns.Add(examDateColumn);

                var accountNumberColumn = new DataGridViewTextBoxColumn();
                accountNumberColumn.DataPropertyName = accountNumberColumn.Name = "AccountNumber";
                accountNumberColumn.HeaderText = "Account Number";
                accountNumberColumn.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                accountNumberColumn.ReadOnly = true;
                columns.Add(accountNumberColumn);

                var examineeNameColumn = new DataGridViewTextBoxColumn();
                examineeNameColumn.DataPropertyName = examineeNameColumn.Name = "Name";
                examineeNameColumn.HeaderText = "Name";
                examineeNameColumn.ReadOnly = true;
                columns.Add(examineeNameColumn);

                var emailColumn = new DataGridViewTextBoxColumn();
                emailColumn.DataPropertyName = emailColumn.Name = "Email";
                emailColumn.HeaderText = "Email";
                emailColumn.ReadOnly = true;
                columns.Add(emailColumn);

                var identification1Column = new DataGridViewButtonColumn();
                identification1Column.DataPropertyName = identification1Column.Name = "Identification1";
                identification1Column.HeaderText = "Identification 1";
                identification1Column.CellTemplate.Style.Font = font;
                columns.Add(identification1Column);

                var identification2Column = new DataGridViewButtonColumn();
                identification2Column.DataPropertyName = identification2Column.Name = "Identification2";
                identification2Column.HeaderText = "Identification 2";
                identification2Column.CellTemplate.Style.Font = font;
                columns.Add(identification2Column);

                var registrationPdfColumn = new DataGridViewButtonColumn();
                registrationPdfColumn.DataPropertyName = registrationPdfColumn.Name = "RegistrationPdf";
                registrationPdfColumn.HeaderText = "Registration Pdf";
                registrationPdfColumn.CellTemplate.Style.Font = font;
                columns.Add(registrationPdfColumn);

                var voucherColumn = new DataGridViewCustomButtonColumn();
                voucherColumn.DataPropertyName = voucherColumn.Name = "Voucher";
                voucherColumn.HeaderText = "Voucher";
                voucherColumn.CellTemplate.Style.Font = font;
                columns.Add(voucherColumn);

                var bookingNumberColumn = new DataGridViewTextBoxColumn();
                bookingNumberColumn.DataPropertyName = bookingNumberColumn.Name = "BookingNumber";
                bookingNumberColumn.HeaderText = "Booking Number";
                bookingNumberColumn.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                bookingNumberColumn.ReadOnly = true;
                columns.Add(bookingNumberColumn);

                var nextStepColumn = new DataGridViewCustomButtonColumn();
                nextStepColumn.DataPropertyName = nextStepColumn.Name = "NextStep";
                nextStepColumn.HeaderText = "Next Step";
                nextStepColumn.CellTemplate.Style.Font = font;
                columns.Add(nextStepColumn);

                var cancelationColumn = new DataGridViewCustomButtonColumn();
                cancelationColumn.DataPropertyName = cancelationColumn.Name = "Cancelation";
                cancelationColumn.HeaderText = "Cancelation";
                cancelationColumn.CellTemplate.Style.Font = font;
                columns.Add(cancelationColumn);
            }
        }
        #endregion

        #region Component Refreshment
        private void RefreshMainViewTable()
        {
            m_isInitializingDgv = true;

            // Set rows
            ApplyFilter_Main();

            m_isInitializingDgv = false;
        }
        #endregion

        #region Data Filtration
        private void ApplyFilter_Main()
        {
            string filterText = txtBx_filter_main.Text;

            DataContainer container = DataContainer.Instance;

            List<RegistrationInfo> registrationInfos = new List<RegistrationInfo>();
            registrationInfos.AddRange(container.RegistrationInfos_Internal);
            registrationInfos.AddRange(container.RegistrationInfos_External);

            var targetDictionary = string.IsNullOrWhiteSpace(filterText) ? registrationInfos : registrationInfos.Where(x => x.Status.ToString().Contains(filterText)
                                                                                                                    || x.Exam.Type.ToString().Contains(filterText)
                                                                                                                    || x.Exam.Date.ToString().Contains(filterText)
                                                                                                                    || x.Email.Contains(filterText));

            if (chckBx_hideEndedExams.Checked)
                targetDictionary = targetDictionary.Where(x => !x.Exam.AlreadyEnded());

            if (chckBx_hideRepeated.Checked)
                targetDictionary = targetDictionary.Where(x => x.Status != eRegistrationStatus.Repeated);

            if (chckBx_hideCanceled.Checked)
                targetDictionary = targetDictionary.Where(x => x.Status != eRegistrationStatus.Canceled);

            var examDictionary = container.NonInstitutionalExam_InternalDictionary;

            int selectingRowIndex = -1;
            var rows = dgv_main.Rows;
            {
                rows.Clear();
                foreach (var registrationInfo in targetDictionary)
                {
                    int examineeId;
                    string name;

                    Person examinee;
                    if (registrationInfo is RegistrationInfo_Internal)
                    {
                        Student examinee_tmp = container.StudentList.FirstOrDefault(x => x.OrganizationEmail == registrationInfo.Email);
                        examinee = examinee_tmp;
                        examineeId = (examinee_tmp != null) ? examinee_tmp.AccountNumber : default;
                        name = (examinee_tmp != null) ? examinee_tmp.Name.ToString() : default;
                    }
                    else
                    {
                        NonIberoCommunityMember examinee_tmp = container.NonIberoCommunityMemberList.FirstOrDefault(x => (x.PersonalId != 0) ? (x.PersonalId == ((RegistrationInfo_External)registrationInfo).PersonalId) : (x.Email == registrationInfo.Email));
                        examinee = examinee_tmp;
                        examineeId = (examinee_tmp != null) ? examinee_tmp.PersonalId : default;
                        name = (examinee_tmp != null) ? examinee_tmp.Name.ToString() : default;
                    }

                    bool alreadyEnded = registrationInfo.Exam.AlreadyEnded();

                    string text_bookingNumber = (registrationInfo.Status != eRegistrationStatus.Repeated && registrationInfo.Status != eRegistrationStatus.WaitingList) ? registrationInfo.BookingNumber.ToString(CoreValues.BookingNumberStringFormat) : "";

                    string buttonText_nextStep = "";
                    if (!alreadyEnded)
                    {
                        switch (registrationInfo.Status)
                        {
                            case eRegistrationStatus.IDsAndPDFSignCheckRequired:
                                buttonText_nextStep = "Send Payment Info";
                                break;

                            case eRegistrationStatus.VoucherCheckRequired:
                                buttonText_nextStep = "Send Student Information Form And Booking Info";
                                break;

                            case eRegistrationStatus.ETSInfoAndBookingCheckRequired:
                                buttonText_nextStep = "Send Last Email";
                                break;

                            default:
                                break;
                        }
                    }

                    string buttonText_cancelation = "";
                    if (!alreadyEnded)
                    {
                        switch (registrationInfo.Status)
                        {
                            case eRegistrationStatus.Repeated:
                            case eRegistrationStatus.WaitingList:
                            case eRegistrationStatus.Canceled:
                                break;

                            default:
                                buttonText_cancelation = "Cancel";
                                break;
                        }
                    }

                    var row = new string[] { registrationInfo.Id.ToString(), registrationInfo.Status.ToString(), registrationInfo.CompletionTime.ToString("dd/MM/yyyy"), registrationInfo.Exam.Type.ToString(), "", examineeId.ToString(),
                                                name, registrationInfo.Email, "View", "View", "View", "View", text_bookingNumber, buttonText_nextStep, buttonText_cancelation };

                    rows.Add(row);

                    var lastRowCells = rows.Last().Cells;

                    {
                        var targetExams = examDictionary.Where(x => x.Value.Type == registrationInfo.Exam.Type && x.Value.IsRegistrationOpen());

                        var cell = ((DataGridViewComboBoxCell)(lastRowCells["ExamDate"]));
                        var items = cell.Items;
                        switch (registrationInfo.Status)
                        {
                            case eRegistrationStatus.WaitingList:
                                {
                                    var initialValue = CoreValues.ComboBox_DefaultString;
                                    items.Add(initialValue);
                                    foreach (var entry in targetExams)
                                    {
                                        var exam = entry.Value;

                                        if ((registrationInfo is RegistrationInfo_Internal)
                                            ? (!container.RegistrationInfos_Internal.Any(x => x.Exam == exam && x.OrganizationEmail == ((Student)examinee).OrganizationEmail))
                                            : (!container.RegistrationInfos_External.Any(x => x.Exam == exam && ((((NonIberoCommunityMember)examinee).PersonalId != 0) 
                                                                                                                    ? (x.PersonalId == ((NonIberoCommunityMember)examinee).PersonalId)
                                                                                                                    : (x.Email == examinee.PreferredEmail))))) // If examinee not registered for the exam yet
                                        {
                                            int registeredExamineesNum = registrationInfos.Where(x => x.Exam == exam).Count();
                                            if (registeredExamineesNum < exam.MaxNumOfExaminees)
                                                items.Add(exam.Date.ToString("dd/MM/yyyy HH:mm"));
                                        }
                                    }
                                    cell.Value = initialValue;
                                }
                                break;

                            case eRegistrationStatus.Repeated:
                                {
                                    var initialValue = "";
                                    items.Add(initialValue);
                                    cell.Value = initialValue;
                                    cell.ReadOnly = true;
                                    cell.Dispose();
                                }
                                break;

                            default:
                                {
                                    var initialValue = registrationInfo.Exam.Date.ToString("dd/MM/yyyy HH:mm");
                                    items.Add(initialValue);
                                    cell.Value = initialValue;
                                    cell.ReadOnly = true;
                                }
                                break;
                        }
                    }

                    if (registrationInfo.VoucherUrl == "")
                    {
                        DataGridViewCustomButtonCell paymentEvidence = lastRowCells["Voucher"] as DataGridViewCustomButtonCell;
                        paymentEvidence.Enabled = false;
                    }

                    if (alreadyEnded
                        || registrationInfo.Status == eRegistrationStatus.Repeated
                        || registrationInfo.Status == eRegistrationStatus.PaymentPending
                        || registrationInfo.Status == eRegistrationStatus.StudentInfoFormPending
                        || registrationInfo.Status == eRegistrationStatus.Completed
                        || registrationInfo.Status == eRegistrationStatus.Canceled)
                    {
                        DataGridViewCustomButtonCell nextStep = lastRowCells["NextStep"] as DataGridViewCustomButtonCell;
                        nextStep.Enabled = false;
                    }

                    if (alreadyEnded
                        || registrationInfo.Status == eRegistrationStatus.Repeated 
                        || registrationInfo.Status == eRegistrationStatus.WaitingList 
                        || registrationInfo.Status == eRegistrationStatus.Canceled)
                    {
                        DataGridViewCustomButtonCell cancelation = lastRowCells["Cancelation"] as DataGridViewCustomButtonCell;
                        cancelation.Enabled = false;
                    }


                    var statusCell = lastRowCells["Status"];
                    if (alreadyEnded)
                    {
                        statusCell.Style.BackColor = Color.DarkGray;
                        statusCell.Style.ForeColor = Color.White;
                    }
                    else
                    {
                        switch (registrationInfo.Status)
                        {
                            case eRegistrationStatus.IDsAndPDFSignCheckRequired:
                                statusCell.Style.BackColor = Color.OrangeRed;
                                break;

                            case eRegistrationStatus.PaymentPending:
                                statusCell.Style.BackColor = Color.PeachPuff;
                                break;

                            case eRegistrationStatus.VoucherCheckRequired:
                                statusCell.Style.BackColor = Color.Gold;
                                break;

                            case eRegistrationStatus.StudentInfoFormPending:
                                statusCell.Style.BackColor = Color.Cornsilk;
                                break;

                            case eRegistrationStatus.ETSInfoAndBookingCheckRequired:
                                statusCell.Style.BackColor = Color.ForestGreen;
                                break;

                            case eRegistrationStatus.Completed:
                                statusCell.Style.BackColor = Color.LightGreen;
                                break;

                            case eRegistrationStatus.Canceled:
                                statusCell.Style.BackColor = Color.Red;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            if (selectingRowIndex != -1)
                rows[selectingRowIndex].Selected = true;
            else if (rows.Count != 0)
                rows[rows.GetLastRow(DataGridViewElementStates.None)].Selected = true;
        }
        #endregion

        #region Shared
        private void LoadPdfAndDisplay(string _url)
        {
            try
            {
                string fileName = Path.GetFileName(_url).Replace("%20", " ");
                string relativeSubFolderPath = _url.Remove("https://iberopuebla.sharepoint.com/sites/CentroInterculturaldeLenguas/Documentos%20compartidos/").Replace("%20", " ").Remove(@"/" + fileName);

                var fileInfo = DataContainer.Instance.SharePointConnectionManager.DownloadFile(fileName, relativeSubFolderPath, CoreValues.LocalSaveFilesPath + @"Data\ImagesAndPdfs\");
                if (fileInfo == null)
                {
                    MessageBox.Show("Failed to load file");
                    return;
                }

                PdfDocument pdfDocument = PdfDocument.FromFile(fileInfo.Path);
                Bitmap[] pageImages = pdfDocument.ToBitmap();

                ImageDisplayer.Display(MergeImagesVertically(pageImages));
            }
            catch (Exception ex)
            {
                MessageBox.Show("File is not a Pdf!");
            }
        }

        private Bitmap MergeImagesVertically(Bitmap[] _images)
        {
            int maxWidth = 0;
            int totalHeight = 0;
            foreach (Bitmap image in _images)
            {
                maxWidth = Math.Max(maxWidth, image.Width);
                totalHeight += image.Height;
            }
            Bitmap result = new Bitmap(maxWidth, totalHeight);
            using (Graphics g = Graphics.FromImage(result))
            {
                int currentHeight = 0;
                foreach (Bitmap image in _images)
                {
                    g.DrawImage(image, 0, currentHeight);
                    currentHeight += image.Height;
                }
            }

            return result;
        }

        private void LoadImageAndDisplay(RegistrationInfo _registrationInfo, string _url)
        {
            if (_registrationInfo is RegistrationInfo_Internal)
                LoadImageAndDisplay_InternalRegistrationInfo(_url);
            else
                LoadImageAndDisplay_ExternalRegistrationInfo(_url);
        }

        private void LoadImageAndDisplay_InternalRegistrationInfo(string _url)
        {
            string fileName = Path.GetFileName(_url).Replace("%20", " ");
            string relativeSubFolderPath = _url.Remove("https://iberopuebla.sharepoint.com/sites/CentroInterculturaldeLenguas/Documentos%20compartidos/").Replace("%20", " ").Remove(@"/" + fileName);

            var fileInfo = DataContainer.Instance.SharePointConnectionManager.DownloadFile(fileName, relativeSubFolderPath, CoreValues.LocalSaveFilesPath + @"Data\ImagesAndPdfs\");
            if (fileInfo == null)
            {
                MessageBox.Show("Failed to load file");
                return;
            }    

            ImageDisplayer.Display(fileInfo.Stream);
        }

        private async void LoadImageAndDisplay_ExternalRegistrationInfo(string _url)
        {
            string fileId = _url.Remove("https://drive.google.com/open?id=");

            var fileInfo = DataContainer.Instance.GoogleDriveConnectionManager.DownloadFile(fileId, CoreValues.LocalSaveFilesPath + @"Data\ImagesAndPdfs\External\");
            if (fileInfo == null)
            {
                MessageBox.Show("Failed to load file");
                return;
            }

            ImageDisplayer.Display(fileInfo.Result.Stream);
        }

        private void ConfirmAndSendEmail(RegistrationInfo_Internal _registrationInfo, string _examineeName)
        {
            if (_registrationInfo == null)
                return;

            eRegistrationStatus status = _registrationInfo.Status;
            string to = _registrationInfo.OrganizationEmail;

            DataContainer container = DataContainer.Instance;

            string body = string.Empty;
            eRegistrationStatus newStatus = status;
            switch (status)
            {
                case eRegistrationStatus.IDsAndPDFSignCheckRequired:
                    {
                        DialogResult dialogResult = MessageBox.Show("Are the IDs and Registration Pdf correct for " + _examineeName + "?", "Caution!", MessageBoxButtons.YesNo);
                        if (dialogResult != DialogResult.Yes)
                        {
                            MessageBox.Show("Action Cancelled!");
                            return;
                        }

                        string cid_1 = "img1_2";
                        string cid_2 = "img1_3";
                        string cid_3 = "img1_4";
                        Dictionary<string, byte[]> inlineImages = new Dictionary<string, byte[]>();
                        inlineImages.Add(cid_1, container.InlineImage1_2);
                        inlineImages.Add(cid_2, container.InlineImage1_3);
                        inlineImages.Add(cid_3, container.InlineImage1_4);

                        body = @"<font size='3.5' color='#3397ff'>Buenos días:<br/>"
                            + "<br/>"
                            + "<br/>"
                            + "Te notifico que toda tu documentación esta correcta, por lo tanto:<br/>"
                            + "<br/>"
                            + "<br/>"
                            + "<br/>"
                            + "<b><u>Te envío el procedimiento para poder realizar el pago:</u></b><br/>"
                            + "<br/>"
                            + "1)      Entrar al siguiente sitio:<br/>"
                            + "<a href=\"http://intrauia.iberopuebla.edu.mx/\">http://intrauia.iberopuebla.edu.mx/</a>"
                            + "<br/>"
                            + "<br/>"
                            + "2)      Accesar con los siguientes datos:<br/>"
                            + "<img src=\"cid:" + cid_1 + "\"></img><br/>"
                            + "<br/>"
                            + "<br/>"
                            + "3)      Ir a la opción Tesorería   →  iCompras<br/>"
                            + "<img src=\"cid:" + cid_2 + "\"></img><br/>"
                            + "<br/>"
                            + "<br/>"
                            + "4)      Buscar la opción Menú → Adeudos<br/>"
                            + "<img src=\"cid:" + cid_3 + "\"></img><br/>"
                            + "<br/>"
                            + "<br/>"
                            + "5)      Dar click en la opción: Pagar $1300.00"
                            + "<br/>"
                            + "<br/>"
                            + "Puedes realizar el pago ya sea directamente en ventanilla de los bancos autorizados, con tarjeta de crédito o débito o mediante transferencia electrónica, si así lo deseas.<br/>"
                            + "<br/>"
                            + "Es importante mencionarte que, en caso de pagar en el banco, el día que generas tu forma de pago debes de realizarlo ya que la referencia cambia todos los días.<br/>"
                            + "<br/>"
                            + "<u><mark><font size='4.5' color='#3397ff'>En cuanto tengas tu comprobante de pago o bien la captura de pantalla de que ya se realizó el pago por favor, debes subirlo a travéz de la siguiente página para seguir con tu registro.</font></mark><br/>"
                            + "<br/>"
                            + "<a href=\"https://forms.office.com/Pages/ResponsePage.aspx?id=XfbdMg_mDUm29Ozusp1f2XjZnkYBPLpMo0bBILvQ08FUOFk0SjAzT0IxSElYTkpTUzhZTklPV0hWQyQlQCN0PWcu\"><font size='4.5' color='#3397ff'>https://forms.office.com/Pages/ResponsePage.aspx?id=XfbdMg_mDUm29Ozusp1f2XjZnkYBPLpMo0bBILvQ08FUOFk0SjAzT0IxSElYTkpTUzhZTklPV0hWQyQlQCN0PWcu</font></a><br/>"
                            + "<br/>"
                            + "<mark><font size='4.5' color='#3397ff'>NOTA : tienes 24 horas para realizar tu pago y poder seguir con tu registro.</font></mark></u><br/>"
                            + "<br/>"
                            + "Quedo a tus órdenes para cualquier duda o información adicional.<br/>"
                            + "<br/>"
                            + "Saludos y buen día</font>";

                        if (CreateAndSendEmailItem("Test", to, body, inlineImages, new List<string>()))
                            MessageBox.Show("Message has been sent");

                        newStatus = eRegistrationStatus.PaymentPending;
                    }
                    break;

                case eRegistrationStatus.VoucherCheckRequired:
                    {
                        DialogResult dialogResult = MessageBox.Show("Is " + _examineeName + "'s voucher valid?", "Caution!", MessageBoxButtons.YesNo);
                        if (dialogResult != DialogResult.Yes)
                        {
                            MessageBox.Show("Action Cancelled!");
                            return;
                        }

                        DateTime examDate = _registrationInfo.Exam.Date;

                        body = @"<font size='3.5' color='#3397ff'>Te adjunto otro PDF""<b>CITAS AIDEL</b>"", que es el procedimiento para poder ingresar al campus, es muy importante que hagas ese procedimiento, sin él no podrás ingresar al campus.<br/>"
                            + "<br/>"
                            + "Fecha: " + examDate.ToString("dddd dd", new CultureInfo("es-Es")) + " de " + examDate.ToString("MMMM yyyy h:mm", new CultureInfo("es-Es")) + examDate.ToString("tt") + "<br/>"
                            + "<br/>"
                            + "Número de folio Booking: <u><i><font size='4.5' color='#3397ff'>" + _registrationInfo.BookingNumber.ToString(CoreValues.BookingNumberStringFormat) + "</font></i></u><br/>"
                            + "<br/>"
                            + "Por último, envío una liga donde deberás ingresar unos datos para poder presentar tu certificación TOEFL.<br/>"
                            + "<br/>"
                            + "<br/>"
                            + "<a href=\"https://forms.office.com/Pages/ResponsePage.aspx?id=XfbdMg_mDUm29Ozusp1f2XjZnkYBPLpMo0bBILvQ08FUQUo2STlOOE5FSFpYUE9SOUg1TjNKRzZaWSQlQCN0PWcu\">https://forms.office.com/Pages/ResponsePage.aspx?id=XfbdMg_mDUm29Ozusp1f2XjZnkYBPLpMo0bBILvQ08FUQUo2STlOOE5FSFpYUE9SOUg1TjNKRzZaWSQlQCN0PWcu</a><br/></font>";

                        if (CreateAndSendEmailItem("Test", to, body, new Dictionary<string, byte[]>(), new List<string>() { "sample.pdf" }))
                            MessageBox.Show("Message has been sent");

                        newStatus = eRegistrationStatus.StudentInfoFormPending;
                    }
                    break;

                case eRegistrationStatus.ETSInfoAndBookingCheckRequired:
                    {
                        DialogResult dialogResult = MessageBox.Show("Has " + _examineeName + " completed his booking process?", "Caution!", MessageBoxButtons.YesNo);
                        if (dialogResult != DialogResult.Yes)
                        {
                            MessageBox.Show("Action Cancelled!");
                            return;
                        }

                        body = @"<font size='3.5' color='#3397ff'>¡Qué buena noticia!, muchas gracias, favor de seguir las indicaciones para presentarte el día del examen, por mi parte seria todo, nos vemos el día tu certificación.<br/>"
                            + "<br/>"
                            + "<b>NOTA:</b>  NO olvides tus audífonos (alámbricos) para tu examen.<br/>"
                            + "<br/>"
                            + "<br/>"
                            + "Como es ya sabido por todos nosotros tenemos el sistema de Automonitoreo Covid en Intrauia que se tiene que llenar una hora antes de entrar al Campus universitario. Por tal motivo se les pide los siguiente:<br/>"
                            + "<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;·         No haber cuidado o convivido en la misma casa con alguna persona sospechosa o confirmada de Covid 19<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;·         No haber estado en contacto directo con una persona sospechosa o confirmada de Covid 19 que no hayas notificado al servicio médico universitario<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;·         No tener síntomas sospechosos de Covid 19:<br/>"
                            + "<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Fiebre (temperatura mayor o igual a 37.5°C)<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Dolor de cabeza intenso que no habías tenido antes<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Dolor o ardor de garganta<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Escurrimiento nasal que no habías presentado antes<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Ojos rojos por motivos que desconoces<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Dolor muscular o en las articulaciones que no tenías antes<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Dolor en el pecho que no habías sentido antes<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Dificultad para respirar<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Tos de reciente aparición<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Tos que va empeorando<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Tos que continua igual<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Diarrea<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Pérdida del olfato<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o   Pérdida del gusto<br/>"
                            + "<br/>"
                            + "&nbsp;&nbsp;&nbsp;&nbsp;·         No tener diagnóstico confirmado de Covid 19<br/>"
                            + "<br/>"
                            + "En caso de que alguna de estas situaciones sea afirmativa un día antes del examen, favor de enviar un correo a aidelexams@iberopuebla.mx y no presentarse a la Universidad. Será reprogramado tu examen de certificación de acuerdo a la siguiente fecha disponible en que ya sea seguro asistir.<br/>"
                            + "<mark><font size='4.5' color='#3397ff'><b>NOTA:</b>  NO olvides hacer tu automonitoreo COVID 19 desde INTRAUIA, 1 hra antes de tu llegada al campus. En caso de no realizarlo no te daran acceso al campus.</font></mark><br/>"
                            + "<br/>"
                            + "<br/>"
                            + "Agradecemos de antemano tu atención y apoyo para que todos sigamos cuidándonos.<br/>"
                            + "<br/>"
                            + "Saludos cordiales,</font>";

                        if (CreateAndSendEmailItem("Test", to, body, new Dictionary<string, byte[]>(), new List<string>()))
                            MessageBox.Show("Message has been sent");

                        newStatus = eRegistrationStatus.Completed;
                    }
                    break;

                case eRegistrationStatus.WaitingList:
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure there is no vacancy for the exam?", "Caution!", MessageBoxButtons.YesNo);
                        if (dialogResult != DialogResult.Yes)
                        {
                            MessageBox.Show("Action Cancelled!");
                            return;
                        }

                        DateTime examDate = _registrationInfo.Exam.Date;

                        body = @"<font size='3.5' color='#3397ff'>Inafortunadamente se han llenado los acientos para el examen de " + examDate.ToString("dddd dd", new CultureInfo("es-Es")) + " de " + examDate.ToString("MMMM yyyy h:mm", new CultureInfo("es-Es")) + "</font>";

                        if (CreateAndSendEmailItem("Test", to, body, new Dictionary<string, byte[]>(), new List<string>()))
                            MessageBox.Show("Message has been sent");

                        newStatus = eRegistrationStatus.Canceled;
                    }
                    break;

                default:
                    break;
            }

            if (newStatus == status)
                return;

            DocumentLoader.EditRegistrationInfoInCSV(_registrationInfo, newStatus);
            if (!container.SharePointConnectionManager.UploadFile(container.FilePath_RegistrationInfo_Internal))
                MessageBox.Show("Error updating registration status!");
        }

        private bool CreateAndSendEmailItem(string _subject,
               string _to, string _body, Dictionary<string, byte[]> _inlineImages, List<string> _attachmentFileNames)
        {
            ExchangeService myservice = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
            myservice.Credentials = new WebCredentials(m_username, m_password);

            try
            {
                string serviceUrl = ConfigurationSettings.AppSettings.Get("Office365WebserivceURL").ToString();
                myservice.Url = new Uri(serviceUrl);
                EmailMessage emailMessage = new EmailMessage(myservice);
                emailMessage.Subject = _subject;
                emailMessage.Body = new MessageBody(_body);
                foreach (var inlineImage in _inlineImages)
                {
                    emailMessage.Attachments.AddFileAttachment(inlineImage.Key, inlineImage.Value);
                    emailMessage.Attachments.Last().IsInline = true;
                    emailMessage.Attachments.Last().ContentId = inlineImage.Key;
                }
                foreach (var attachmentFileName in _attachmentFileNames)
                {
                    emailMessage.Attachments.AddFileAttachment(CoreValues.LocalSaveFilesPath + @"Attachments\" + attachmentFileName);
                    emailMessage.Attachments.Last().IsInline = false;
                }
                emailMessage.ToRecipients.Add(_to);
                emailMessage.SendAndSaveCopy();

                return true;
            }
            catch (SmtpException exception)
            {
                string msg = "Mail cannot be sent (SmtpException):"
                        + exception.Message;

                MessageBox.Show(msg);
            }
            catch (AutodiscoverRemoteException exception)
            {
                string msg = "Mail cannot be sent (AutodiscoverRemoteException):"
                        + exception.Message;

                MessageBox.Show(msg);
            }

            return false;
        }

        private void CancelRegistration(RegistrationInfo_Internal _registrationInfo, string _examineeName)
        {
            if (_registrationInfo == null)
                return;

            eRegistrationStatus status = _registrationInfo.Status;
            string to = _registrationInfo.OrganizationEmail;

            DataContainer container = DataContainer.Instance;

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel registration of " + _examineeName + " to the exam conducted on " + _registrationInfo.Exam.Date.ToString("MMMM dd, yyyy") + "?", "Caution!", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes)
            {
                MessageBox.Show("Action Cancelled!");
                return;
            }

            DocumentLoader.EditRegistrationInfoInCSV(_registrationInfo, eRegistrationStatus.Canceled);
            if (!container.SharePointConnectionManager.UploadFile(container.FilePath_RegistrationInfo_Internal))
                MessageBox.Show("Error updating registration status!");
        }

        private DialogResult ShowInputDialogBox(ref string input, string prompt, string title = "Title", int width = 300, int height = 200)
        {
            //This function creates the custom input dialog box by individually creating the different window elements and adding them to the dialog box

            //Specify the size of the window using the parameters passed
            Size size = new Size(width, height);
            //Create a new form using a System.Windows Form
            WFForm inputBox = new WFForm();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            //Set the window title using the parameter passed
            inputBox.Text = title;

            //Create a new label to hold the prompt
            Label label = new Label();
            label.Text = prompt;
            label.Location = new Point(5, 5);
            label.Width = size.Width - 10;
            inputBox.Controls.Add(label);

            //Create a textbox to accept the user's input
            TextBox textBox = new TextBox();
            textBox.Size = new Size(size.Width - 10, 23);
            textBox.Location = new Point(5, label.Location.Y + 20);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            //Create an OK Button 
            Button confirmButton = new Button();
            confirmButton.DialogResult = DialogResult.OK;
            confirmButton.Name = "confirmButton";
            confirmButton.Size = new Size(75, 23);
            confirmButton.Text = "Confirm";
            confirmButton.Location = new Point(size.Width - 80 - 80, size.Height - 30);
            inputBox.Controls.Add(confirmButton);

            //Create a Cancel Button
            Button cancelButton = new Button();
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(size.Width - 80, size.Height - 30);
            inputBox.Controls.Add(cancelButton);

            //Set the input box's buttons to the created OK and Cancel Buttons respectively so the window appropriately behaves with the button clicks
            inputBox.AcceptButton = confirmButton;
            inputBox.CancelButton = cancelButton;

            //Move the window dialog box
            Size s = this.Size;
            Point l = this.Location;
            inputBox.Left = l.X + ((s.Width - size.Width) / 2);
            inputBox.Top = l.Y + ((s.Height - size.Height) / 2);
            //Show the window dialog box 
            DialogResult result = inputBox.ShowDialog();

            input = textBox.Text;

            //After input has been submitted, return the input value
            return result;
        }

        private class RowComparer : System.Collections.IComparer
        {
            private static int m_sortOrderModifier = 1;

            public RowComparer(SortOrder _sortOrder)
            {
                if (_sortOrder == SortOrder.Descending)
                    m_sortOrderModifier = -1;
                else if (_sortOrder == SortOrder.Ascending)
                    m_sortOrderModifier = 1;
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow dataGridViewRow1 = (DataGridViewRow)x;
                DataGridViewRow dataGridViewRow2 = (DataGridViewRow)y;

                // Try to sort based on the Value column.
                int compareResult = Convert.ToInt32(dataGridViewRow1.Cells[0].Value.ToString())
                                            .CompareTo(Convert.ToInt32(dataGridViewRow2.Cells[0].Value.ToString()));

                return compareResult * m_sortOrderModifier;
            }
        }
        #endregion

        private void btn_openExcelFileFolder_Click(object sender, EventArgs e)
        {
            Process.Start(CoreValues.LocalSaveFilesPath + @"Excel");
        }

        private void chckBx_hideRepeated_CheckedChanged(object sender, EventArgs e)
        {
            RefreshMainViewTable();
        }

        private void chckBx_hideCanceled_CheckedChanged(object sender, EventArgs e)
        {
            RefreshMainViewTable();
        }

        private void chckBx_hideEndedExams_CheckedChanged(object sender, EventArgs e)
        {
            RefreshMainViewTable();
        }
    }
}