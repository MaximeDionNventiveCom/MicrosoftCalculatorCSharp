// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CalculatorApp;
using CalculatorApp.Common;
using CalculatorApp.Common.Automation;
using CalculatorApp.ViewModel;
using CalculationManager;
using Platform;
using Platform.Collections;
using std;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.Storage.Streams;
using Windows.Foundation.Collections;
using Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;

namespace CalculatorApp.ViewModel
{
    public class StandardCalculatorViewModel : INotifyPropertyChanged
    {
        int StandardModePrecision = 16;
        int ScientificModePrecision = 32;
        int ProgrammerModePrecision = 64;

        const string IsStandardPropertyName = "IsStandard";
        const string IsScientificPropertyName = "IsScientific";
        const string IsProgrammerPropertyName = "IsProgrammer";
        const string DisplayValuePropertyName = "DisplayValue";
        const string CalculationResultAutomationNamePropertyName = "CalculationResultAutomationName";

        const string CalculatorExpression = "Format_CalculatorExpression";
        const string CalculatorResults = "Format_CalculatorResults";
        const string CalculatorResults_DecimalSeparator_Announced = "Format_CalculatorResults_Decimal";
        const string HexButton = "Format_HexButtonValue";
        const string DecButton = "Format_DecButtonValue";
        const string OctButton = "Format_OctButtonValue";
        const string BinButton = "Format_BinButtonValue";
        const string OpenParenthesisCountAutomationFormat = "Format_OpenParenthesisCountAutomationNamePix";
        const string NoParenthesisAdded = "NoRightParenthesisAdded_Announcement";
        const string MaxDigitsReachedFormat = "Format_MaxDigitsReached";
        const string ButtonPressFeedbackFormat = "Format_ButtonPressAuditoryFeedback";
        const string MemorySave = "Format_MemorySave";
        const string MemoryItemChanged = "Format_MemorySlotChanged";
        const string MemoryItemCleared = "Format_MemorySlotCleared";
        const string MemoryCleared = "Memory_Cleared";
        const string DisplayCopied = "Display_Copied";

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string p = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p)); OnPropertyChanged(p); }
        private string m_DisplayValue;
        public string DisplayValue { get => m_DisplayValue; set { m_DisplayValue = value; RaisePropertyChanged("DisplayValue"); } }


        private HistoryViewModel m_HistoryVM;
        public HistoryViewModel HistoryVM { get => m_HistoryVM; set { m_HistoryVM = value; RaisePropertyChanged("HistoryVM"); } }


        private const string IsInErrorName = "IsInError";
        private bool m_IsInError;
        public bool IsInError { get => m_IsInError; set { m_IsInError = value; RaisePropertyChanged("IsInError"); } }


        private bool m_IsOperatorCommand;
        public bool IsOperatorCommand { get => m_IsOperatorCommand; set { m_IsOperatorCommand = value; RaisePropertyChanged("IsOperatorCommand"); } }


        private string m_DisplayStringExpression;
        public string DisplayStringExpression { get => m_DisplayStringExpression; set { m_DisplayStringExpression = value; RaisePropertyChanged("DisplayStringExpression"); } }


        private Windows.Foundation.Collections.IObservableList<Common.DisplayExpressionToken> m_ExpressionTokens;
        public Windows.Foundation.Collections.IObservableList<Common.DisplayExpressionToken> ExpressionTokens { get => m_ExpressionTokens; private set { m_ExpressionTokens = value; RaisePropertyChanged("ExpressionTokens"); } }


        private string m_DecimalDisplayValue;
        public string DecimalDisplayValue { get => m_DecimalDisplayValue; set { m_DecimalDisplayValue = value; RaisePropertyChanged("DecimalDisplayValue"); } }


        private string m_HexDisplayValue;
        public string HexDisplayValue { get => m_HexDisplayValue; set { m_HexDisplayValue = value; RaisePropertyChanged("HexDisplayValue"); } }


        private string m_OctalDisplayValue;
        public string OctalDisplayValue { get => m_OctalDisplayValue; set { m_OctalDisplayValue = value; RaisePropertyChanged("OctalDisplayValue"); } }


        private const string BinaryDisplayValueName = "BinaryDisplayValue";
        private string m_BinaryDisplayValue;
        public string BinaryDisplayValue { get => m_BinaryDisplayValue; set { m_BinaryDisplayValue = value; RaisePropertyChanged("BinaryDisplayValue"); } }


        private string m_HexDisplayValue_AutomationName;
        public string HexDisplayValue_AutomationName { get => m_HexDisplayValue_AutomationName; set { m_HexDisplayValue_AutomationName = value; RaisePropertyChanged("HexDisplayValue_AutomationName"); } }


        private string m_DecDisplayValue_AutomationName;
        public string DecDisplayValue_AutomationName { get => m_DecDisplayValue_AutomationName; set { m_DecDisplayValue_AutomationName = value; RaisePropertyChanged("DecDisplayValue_AutomationName"); } }


        private string m_OctDisplayValue_AutomationName;
        public string OctDisplayValue_AutomationName { get => m_OctDisplayValue_AutomationName; set { m_OctDisplayValue_AutomationName = value; RaisePropertyChanged("OctDisplayValue_AutomationName"); } }


        private string m_BinDisplayValue_AutomationName;
        public string BinDisplayValue_AutomationName { get => m_BinDisplayValue_AutomationName; set { m_BinDisplayValue_AutomationName = value; RaisePropertyChanged("BinDisplayValue_AutomationName"); } }


        private bool m_IsBinaryOperatorEnabled;
        public bool IsBinaryOperatorEnabled { get => m_IsBinaryOperatorEnabled; set { m_IsBinaryOperatorEnabled = value; RaisePropertyChanged("IsBinaryOperatorEnabled"); } }


        private bool m_IsUnaryOperatorEnabled;
        public bool IsUnaryOperatorEnabled { get => m_IsUnaryOperatorEnabled; set { m_IsUnaryOperatorEnabled = value; RaisePropertyChanged("IsUnaryOperatorEnabled"); } }


        private bool m_IsNegateEnabled;
        public bool IsNegateEnabled { get => m_IsNegateEnabled; set { m_IsNegateEnabled = value; RaisePropertyChanged("IsNegateEnabled"); } }


        private bool m_IsDecimalEnabled;
        public bool IsDecimalEnabled { get => m_IsDecimalEnabled; set { m_IsDecimalEnabled = value; RaisePropertyChanged("IsDecimalEnabled"); } }


        private bool m_IsCurrentViewPinned;
        public bool IsCurrentViewPinned { get => m_IsCurrentViewPinned; set { m_IsCurrentViewPinned = value; RaisePropertyChanged("IsCurrentViewPinned"); } }


        private MemoryItemViewModel[] m_MemorizedNumbers;
        public MemoryItemViewModel[] MemorizedNumbers { get => m_MemorizedNumbers; set { m_MemorizedNumbers = value; RaisePropertyChanged("MemorizedNumbers"); } }


        private const string IsMemoryEmptyName = "IsMemoryEmpty";
        private bool m_IsMemoryEmpty;
        public bool IsMemoryEmpty { get => m_IsMemoryEmpty; set { m_IsMemoryEmpty = value; RaisePropertyChanged("IsMemoryEmpty"); } }


        private bool m_IsFToEChecked;
        public bool IsFToEChecked { get => m_IsFToEChecked; set { m_IsFToEChecked = value; RaisePropertyChanged("IsFToEChecked"); } }


        private bool m_IsFToEEnabled;
        public bool IsFToEEnabled { get => m_IsFToEEnabled; set { m_IsFToEEnabled = value; RaisePropertyChanged("IsFToEEnabled"); } }


        private bool m_IsHyperbolicChecked;
        public bool IsHyperbolicChecked { get => m_IsHyperbolicChecked; set { m_IsHyperbolicChecked = value; RaisePropertyChanged("IsHyperbolicChecked"); } }


        private bool m_AreHEXButtonsEnabled;
        public bool AreHEXButtonsEnabled { get => m_AreHEXButtonsEnabled; set { m_AreHEXButtonsEnabled = value; RaisePropertyChanged("AreHEXButtonsEnabled"); } }


        private string m_CalculationResultAutomationName;
        public string CalculationResultAutomationName { get => m_CalculationResultAutomationName; set { m_CalculationResultAutomationName = value; RaisePropertyChanged("CalculationResultAutomationName"); } }


        private string m_CalculationExpressionAutomationName;
        public string CalculationExpressionAutomationName { get => m_CalculationExpressionAutomationName; set { m_CalculationExpressionAutomationName = value; RaisePropertyChanged("CalculationExpressionAutomationName"); } }


        private bool m_IsShiftProgrammerChecked;
        public bool IsShiftProgrammerChecked { get => m_IsShiftProgrammerChecked; set { m_IsShiftProgrammerChecked = value; RaisePropertyChanged("IsShiftProgrammerChecked"); } }


        private bool m_IsQwordEnabled;
        public bool IsQwordEnabled { get => m_IsQwordEnabled; set { m_IsQwordEnabled = value; RaisePropertyChanged("IsQwordEnabled"); } }


        private bool m_IsDwordEnabled;
        public bool IsDwordEnabled { get => m_IsDwordEnabled; set { m_IsDwordEnabled = value; RaisePropertyChanged("IsDwordEnabled"); } }


        private bool m_IsWordEnabled;
        public bool IsWordEnabled { get => m_IsWordEnabled; set { m_IsWordEnabled = value; RaisePropertyChanged("IsWordEnabled"); } }


        private bool m_IsByteEnabled;
        public bool IsByteEnabled { get => m_IsByteEnabled; set { m_IsByteEnabled = value; RaisePropertyChanged("IsByteEnabled"); } }


        private int m_CurrentRadixType;
        public int CurrentRadixType { get => m_CurrentRadixType; set { m_CurrentRadixType = value; RaisePropertyChanged("CurrentRadixType"); } }


        private bool m_AreTokensUpdated;
        public bool AreTokensUpdated { get => m_AreTokensUpdated; set { m_AreTokensUpdated = value; RaisePropertyChanged("AreTokensUpdated"); } }


        private bool m_AreHistoryShortcutsEnabled;
        public bool AreHistoryShortcutsEnabled { get => m_AreHistoryShortcutsEnabled; set { m_AreHistoryShortcutsEnabled = value; RaisePropertyChanged("AreHistoryShortcutsEnabled"); } }


        private bool m_AreProgrammerRadixOperatorsEnabled;
        public bool AreProgrammerRadixOperatorsEnabled { get => m_AreProgrammerRadixOperatorsEnabled; set { m_AreProgrammerRadixOperatorsEnabled = value; RaisePropertyChanged("AreProgrammerRadixOperatorsEnabled"); } }


        private CalculatorApp.Common.Automation.NarratorAnnouncement m_Announcement;
        public CalculatorApp.Common.Automation.NarratorAnnouncement Announcement { get => m_Announcement; set { m_Announcement = value; RaisePropertyChanged("Announcement"); } }


        private uint m_OpenParenthesisCount;
        public uint OpenParenthesisCount { get => m_OpenParenthesisCount; private set { m_OpenParenthesisCount = value; RaisePropertyChanged("OpenParenthesisCount"); } }



        public ICommand CopyCommand { get; } = new DelegateCommand(OnCopyCommand);


        public ICommand PasteCommand { get; } = new DelegateCommand(OnPasteCommand);


        public ICommand ButtonPressed { get; } = new DelegateCommand(OnButtonPressed);


        public ICommand ClearMemoryCommand { get; } = new DelegateCommand(OnClearMemoryCommand);


        public ICommand MemoryItemPressed { get; } = new DelegateCommand(OnMemoryItemPressed);


        public ICommand MemoryAdd { get; } = new DelegateCommand(OnMemoryAdd);


        public ICommand MemorySubtract { get; } = new DelegateCommand(OnMemorySubtract);

        //
        //
        //

        private NumbersAndOperatorsEnum m_CurrentAngleType;
        private char m_decimalSeparator;
        private CalculatorDisplay m_calculatorDisplay;
        private EngineResourceProvider m_resourceProvider;
        private CalculatorManager m_standardCalculatorManager;
        private string m_expressionAutomationNameFormat;
        private string m_localizedCalculationResultAutomationFormat;
        private string m_localizedCalculationResultDecimalAutomationFormat;
        private string m_localizedHexaDecimalAutomationFormat;
        private string m_localizedDecimalAutomationFormat;
        private string m_localizedOctalAutomationFormat;
        private string m_localizedBinaryAutomationFormat;
        private string m_localizedMaxDigitsReachedAutomationFormat;
        private string m_localizedButtonPressFeedbackAutomationFormat;
        private string m_localizedMemorySavedAutomationFormat;
        private string m_localizedMemoryItemChangedAutomationFormat;
        private string m_localizedMemoryItemClearedAutomationFormat;
        private string m_localizedMemoryCleared;
        private string m_localizedOpenParenthesisCountChangedAutomationFormat;
        private string m_localizedNoRightParenthesisAddedFormat;

        private bool m_pinned;
        private bool m_isOperandEnabled;
        private bool m_isEditingEnabled;
        private bool m_isStandard;
        private bool m_isScientific;
        private bool m_isProgrammer;
        private bool m_isBinaryBitFlippingEnabled;
        private bool m_isBitFlipChecked;
        private bool m_isShiftChecked;
        private bool m_isRtlLanguage;
        private int m_tokenPosition;
        private bool m_keyPressed;
        private bool m_operandUpdated;
        private bool m_completeTextSelection;
        private bool m_isLastOperationHistoryLoad;
        private string m_selectedExpressionLastData;
        private DisplayExpressionToken m_selectedExpressionToken;


        public StandardCalculatorViewModel()
        {
            m_DisplayValue = "0";
            m_DecimalDisplayValue = "0";
            m_HexDisplayValue = "0";
            m_BinaryDisplayValue = "0";
            m_OctalDisplayValue = "0";
            // m_standardCalculatorManager = &m_calculatorDisplay, &m_resourceProvider;
            m_ExpressionTokens = new List<DisplayExpressionToken>();
            m_MemorizedNumbers = new List<MemoryItemViewModel ();
            m_IsMemoryEmpty = true;
            m_IsFToEChecked = false;
            m_isShiftChecked = false;
            m_IsShiftProgrammerChecked = false;
            m_IsQwordEnabled = true;
            m_IsDwordEnabled = true;
            m_IsWordEnabled = true;
            m_IsByteEnabled = true;
            m_isBitFlipChecked = false;
            m_isBinaryBitFlippingEnabled = false;
            m_CurrentRadixType = RADIX_TYPE.DEC_RADIX;
            m_CurrentAngleType = NumbersAndOperatorsEnum.Degree;
            m_Announcement = null;
            m_OpenParenthesisCount = 0;
            m_feedbackForButtonPress = null;
            m_isRtlLanguage = false;
            m_localizedMaxDigitsReachedAutomationFormat = null;
            m_localizedButtonPressFeedbackAutomationFormat = null;
            m_localizedMemorySavedAutomationFormat = null;
            m_localizedMemoryItemChangedAutomationFormat = null;
            m_localizedMemoryItemClearedAutomationFormat = null;
            m_localizedMemoryCleared = null;
            m_localizedOpenParenthesisCountChangedAutomationFormat = null;
            m_localizedNoRightParenthesisAddedFormat = null;


            WeakReference calculatorViewModel(this);
            m_calculatorDisplay.SetCallback(calculatorViewModel);
            m_expressionAutomationNameFormat = AppResourceProvider.GetInstance().GetResourceString(CalculatorResourceKeys.CalculatorExpression);
            m_localizedCalculationResultAutomationFormat = AppResourceProvider.GetInstance().GetResourceString(CalculatorResourceKeys.CalculatorResults);
            m_localizedCalculationResultDecimalAutomationFormat =
                AppResourceProvider.GetInstance().GetResourceString(CalculatorResourceKeys.CalculatorResults_DecimalSeparator_Announced);
            m_localizedHexaDecimalAutomationFormat = AppResourceProvider.GetInstance().GetResourceString(CalculatorResourceKeys.HexButton);
            m_localizedDecimalAutomationFormat = AppResourceProvider.GetInstance().GetResourceString(CalculatorResourceKeys.DecButton);
            m_localizedOctalAutomationFormat = AppResourceProvider.GetInstance().GetResourceString(CalculatorResourceKeys.OctButton);
            m_localizedBinaryAutomationFormat = AppResourceProvider.GetInstance().GetResourceString(CalculatorResourceKeys.BinButton);

            // Initialize the Automation Name
            CalculationResultAutomationName = GetLocalizedStringFormat(m_localizedCalculationResultAutomationFormat, m_DisplayValue);
            CalculationExpressionAutomationName = GetLocalizedStringFormat(m_expressionAutomationNameFormat, "");

            // Initialize history view model
            m_HistoryVM = new HistoryViewModel(&m_standardCalculatorManager);
            m_HistoryVM.SetCalculatorDisplay(m_calculatorDisplay);

            m_decimalSeparator = LocalizationSettings.GetInstance().GetDecimalSeparator();

            if (CoreWindow.GetForCurrentThread() != null)
            {
                // Must have a CoreWindow to access the resource context.
                m_isRtlLanguage = LocalizationService.GetInstance().IsRtlLayout();
            }

            IsEditingEnabled = false;
            IsUnaryOperatorEnabled = true;
            IsBinaryOperatorEnabled = true;
            IsOperandEnabled = true;
            IsNegateEnabled = true;
            IsDecimalEnabled = true;
            AreHistoryShortcutsEnabled = true;
            AreProgrammerRadixOperatorsEnabled = false;

            m_tokenPosition = -1;
            m_isLastOperationHistoryLoad = false;
        }

        String LocalizeDisplayValue(string displayValue, bool isError)
        {
            string result = displayValue;

            LocalizationSettings.GetInstance().LocalizeDisplayValue(&result);

            // WINBLUE: 440747 - In BiDi languages, error messages need to be wrapped in LRE/PDF
            if (isError && m_isRtlLanguage)
            {
                result.insert(result.begin(), Utils.LRE);
                result.push_back(Utils.PDF);
            }

            return new Platform.String(result);
        }

        String CalculateNarratorDisplayValue(string displayValue, String localizedDisplayValue, bool isError)
        {
            String localizedValue = localizedDisplayValue;
            String varmationFormat = m_localizedCalculationResultAutomationFormat;

            // The narrator doesn't read the decimalSeparator if it's the last character
            if (Utils.IsLastCharacterTarget(displayValue, m_decimalSeparator))
            {
                // remove the decimal separator, to avoid a long pause between words
                localizedValue = LocalizeDisplayValue(displayValue.substr(0, displayValue.length() - 1), isError);

                // Use a format which has a word in the decimal separator's place
                // "The Display is 10 point"
                varmationFormat = m_localizedCalculationResultDecimalAutomationFormat;
            }

            // In Programmer modes using non-base10, we want the strings to be read as literal digits.
            if (IsProgrammer && CurrentRadixType != RADIX_TYPE.DEC_RADIX)
            {
                localizedValue = GetNarratorStringReadRawNumbers(localizedValue);
            }

            return GetLocalizedStringFormat(varmationFormat, localizedValue);
        }

        String GetNarratorStringReadRawNumbers(String localizedDisplayValue)
        {
            stringstream wss;
            var locSettings = LocalizationSettings.GetInstance();

            // Insert a space after each digit in the string, to force Narrator to read them as separate numbers.
            string wstrValue(localizedDisplayValue.Data());
            for (char & c : wstrValue)
            {
                wss << c;
                if (locSettings.IsLocalizedHexDigit(c))
                {
                    wss << ' ';
                }
            }

            return new String(wss.str());
        }

        void SetPrimaryDisplay(string displayStringValue, bool isError)
        {
            String localizedDisplayStringValue = LocalizeDisplayValue(displayStringValue, isError);

            // Set this variable before the DisplayValue is modified, Otherwise the DisplayValue will
            // not match what the narrator is saying
            m_CalculationResultAutomationName = CalculateNarratorDisplayValue(displayStringValue, localizedDisplayStringValue, isError);

            DisplayValue = localizedDisplayStringValue;

            IsInError = isError;

            if (IsProgrammer)
            {
                UpdateProgrammerPanelDisplay();
            }
        }

        void DisplayPasteError()
        {
            m_standardCalculatorManager.DisplayPasteError();
        }

        void SetParenthesisCount(uint parenthesisCount)
        {
            if (m_OpenParenthesisCount == parenthesisCount)
            {
                return;
            }

            OpenParenthesisCount = parenthesisCount;
            if (IsProgrammer || IsScientific)
            {
                SetOpenParenthesisCountNarratorAnnouncement();
            }
        }

        void SetOpenParenthesisCountNarratorAnnouncement()
        {
            string localizedParenthesisCount = to_string(m_OpenParenthesisCount);
            LocalizationSettings.GetInstance().LocalizeDisplayValue(&localizedParenthesisCount);

            String announcement = LocalizationStringUtil.GetLocalizedNarratorAnnouncement(
                CalculatorResourceKeys.OpenParenthesisCountAutomationFormat,
                m_localizedOpenParenthesisCountChangedAutomationFormat,
                localizedParenthesisCount);

            Announcement = CalculatorAnnouncement.GetOpenParenthesisCountChangedAnnouncement(announcement);
        }

        void OnNoRightParenAdded()
        {
            SetNoParenAddedNarratorAnnouncement();
        }

        void SetNoParenAddedNarratorAnnouncement()
        {
            String announcement =
                LocalizationStringUtil.GetLocalizedNarratorAnnouncement(CalculatorResourceKeys.NoParenthesisAdded, m_localizedNoRightParenthesisAddedFormat);

            Announcement = CalculatorAnnouncement.GetNoRightParenthesisAddedAnnouncement(announcement);
        }

        void DisableButtons(CommandType selectedExpressionCommandType)
        {
            if (selectedExpressionCommandType == CommandType.OperandCommand)
            {
                IsBinaryOperatorEnabled = false;
                IsUnaryOperatorEnabled = false;
                IsOperandEnabled = true;
                IsNegateEnabled = true;
                IsDecimalEnabled = true;
            }
            if (selectedExpressionCommandType == CommandType.BinaryCommand)
            {
                IsBinaryOperatorEnabled = true;
                IsUnaryOperatorEnabled = false;
                IsOperandEnabled = false;
                IsNegateEnabled = false;
                IsDecimalEnabled = false;
            }
            if (selectedExpressionCommandType == CommandType.UnaryCommand)
            {
                IsBinaryOperatorEnabled = false;
                IsUnaryOperatorEnabled = true;
                IsOperandEnabled = false;
                IsNegateEnabled = true;
                IsDecimalEnabled = false;
            }
        }

        void SetExpressionDisplay(CalculatorList<Pair<string, int>> tokens, CalculatorList<IExpressionCommand> commands)
        {
            m_tokens = tokens;
            m_commands = commands;
            if (!IsEditingEnabled)
            {
                SetTokens(tokens);
            }

            CalculationExpressionAutomationName = GetCalculatorExpressionAutomationName();

            AreTokensUpdated = true;
        }

        void SetHistoryExpressionDisplay(
             CalculatorList<Pair<string, int>> tokens,
            CalculatorList<IExpressionCommand> commands)
        {
            m_tokens = new CalculatorList<Pair<string, int>>(tokens);
            m_commands = new CalculatorList<IExpressionCommand>(commands);
            IsEditingEnabled = false;

            // Setting the History Item Load Mode so that UI does not get updated with recalculation of every token
            m_standardCalculatorManager.SetInHistoryItemLoadMode(true);
            Recalculate(true);
            m_standardCalculatorManager.SetInHistoryItemLoadMode(false);
            m_isLastOperationHistoryLoad = true;
        }

        void SetTokens(CalculatorList<pair<string, int>> tokens)
        {
            AreTokensUpdated = false;

            uint nTokens = 0;
            tokens.GetSize(&nTokens);

            if (nTokens == 0)
            {
                m_ExpressionTokens.Clear();
                return;
            }

            Pair<string, int> currentToken;
            var localizer = LocalizationSettings.GetInstance();

            const string separator = " ";
            for (uint i = 0; i < nTokens; ++i)
            {
                if (SUCCEEDED(tokens.GetAt(i, &currentToken)))
                {
                    Common.TokenType type;
                    bool isEditable = (currentToken.second == -1) ? false : true;
                    localizer.LocalizeDisplayValue(&(currentToken.first));

                    if (!isEditable)
                    {
                        type = currentToken.first == separator ? TokenType.Separator : TokenType.Operator;
                    }
                    else
                    {
                        IExpressionCommand command;
                        IFTPlatformException(m_commands.GetAt(currentToken.second, &command));
                        type = command.GetCommandType() == CommandType.OperandCommand ? TokenType.Operand : TokenType.Operator;
                    }

                    var currentTokenString = new String(currentToken.first);
                    if (i < m_ExpressionTokens.Size)
                    {
                        var existingItem = m_ExpressionTokens.GetAt(i);
                        if (type == existingItem.Type && existingItem.Token.Equals(currentTokenString))
                        {
                            existingItem.TokenPosition = i;
                            existingItem.IsTokenEditable = isEditable;
                            existingItem.CommandIndex = 0;
                        }
                        else
                        {
                            var expressionToken = new DisplayExpressionToken(currentTokenString, i, isEditable, type);
                            m_ExpressionTokens.InsertAt(i, expressionToken);
                        }
                    }
                    else
                    {
                        var expressionToken = new DisplayExpressionToken(currentTokenString, i, isEditable, type);
                        m_ExpressionTokens.Append(expressionToken);
                    }
                }
            }

            while (m_ExpressionTokens.Size != nTokens)
            {
                m_ExpressionTokens.RemoveAtEnd();
            }
        }

        String GetCalculatorExpressionAutomationName()
        {
            String expression = "";
            for (var && token : m_ExpressionTokens)
            {
                expression += LocalizationService.GetNarratorReadableToken(token.Token);
            }

            return GetLocalizedStringFormat(m_expressionAutomationNameFormat, expression);
        }

        void SetMemorizedNumbers(string[] newMemorizedNumbers)
        {
            var localizer = LocalizationSettings.GetInstance();
            if (newMemorizedNumbers.size() == 0) // Memory has been cleared
            {
                MemorizedNumbers.Clear();
                IsMemoryEmpty = true;
            }
            // A new value is added to the memory
            else if (newMemorizedNumbers.size() > MemorizedNumbers.Size)
            {
                while (newMemorizedNumbers.size() > MemorizedNumbers.Size)
                {
                    int newValuePosition = newMemorizedNumbers.size() - MemorizedNumbers.Size - 1;
                    var stringValue = newMemorizedNumbers.at(newValuePosition);

                    MemoryItemViewModel memorySlot = new MemoryItemViewModel(this);
                    memorySlot.Position = 0;
                    localizer.LocalizeDisplayValue(&stringValue);
                    memorySlot.Value = Utils.LRO + new String(stringValue) + Utils.PDF;

                    MemorizedNumbers.InsertAt(0, memorySlot);
                    IsMemoryEmpty = false;

                    // Update the slot position for the rest of the slots
                    for (uint i = 1; i < MemorizedNumbers.Size; i++)
                    {
                        MemorizedNumbers.GetAt(i).Position++;
                    }
                }
            }
            else if (newMemorizedNumbers.size() == MemorizedNumbers.Size) // Either M+ or M-
            {
                for (uint i = 0; i < MemorizedNumbers.Size; i++)
                {
                    var nestringValue = newMemorizedNumbers.at(i);
                    localizer.LocalizeDisplayValue(&nestringValue);

                    // If the value is different, update the value
                    if (MemorizedNumbers.GetAt(i).Value != string(nestringValue))
                    {
                        MemorizedNumbers.GetAt(i).Value = Utils.LRO + new String(nestringValue) + Utils.PDF;
                    }
                }
            }
        }

        void FtoEButtonToggled()
        {
            OnButtonPressed(NumbersAndOperatorsEnum.FToE);
        }

        void HandleUpdatedOperandData(Command cmdenum)
        {
            DisplayExpressionToken displayExpressionToken = ExpressionTokens.GetAt(m_tokenPosition);
            if (displayExpressionToken == null)
            {
                return;
            }
            if ((displayExpressionToken.Token == null) || (displayExpressionToken.Token.Length() == 0))
            {
                displayExpressionToken.CommandIndex = 0;
            }

            char ch = 0;
            if ((cmdenum >= Command.Command0) && (cmdenum <= Command.Command9))
            {
                switch (cmdenum)
                {
                    case Command.Command0:
                        ch = '0';
                        break;
                    case Command.Command1:
                        ch = '1';
                        break;
                    case Command.Command2:
                        ch = '2';
                        break;
                    case Command.Command3:
                        ch = '3';
                        break;
                    case Command.Command4:
                        ch = '4';
                        break;
                    case Command.Command5:
                        ch = '5';
                        break;
                    case Command.Command6:
                        ch = '6';
                        break;
                    case Command.Command7:
                        ch = '7';
                        break;
                    case Command.Command8:
                        ch = '8';
                        break;
                    case Command.Command9:
                        ch = '9';
                        break;
                }
            }
            else if (cmdenum == Command.CommandPNT)
            {
                ch = '.';
            }
            else if (cmdenum == Command.CommandBACK)
            {
                ch = 'x';
            }
            else
            {
                return;
            }

            int length = 0;
            char* temp = new char[100];
            const char* data = m_selectedExpressionLastData.Data();
            int i = 0, j = 0;
            int commandIndex = displayExpressionToken.CommandIndex;

            if (IsOperandTextCompletelySelected)
            {
                // Clear older text;
                m_selectedExpressionLastData = "";
                if (ch == 'x')
                {
                    temp[0] = '\0';
                    commandIndex = 0;
                }
                else
                {
                    temp[0] = ch;
                    temp[1] = '\0';
                    commandIndex = 1;
                }
                IsOperandTextCompletelySelected = false;
            }
            else
            {
                if (ch == 'x')
                {
                    if (commandIndex == 0)
                    {
                        delete[] temp;
                        return;
                    }

                    length = m_selectedExpressionLastData.Length();
                    for (; j < length; ++j)
                    {
                        if (j == commandIndex - 1)
                        {
                            continue;
                        }
                        temp[i++] = data[j];
                    }
                    temp[i] = '\0';
                    commandIndex -= 1;
                }
                else
                {
                    length = m_selectedExpressionLastData.Length() + 1;
                    if (length > 50)
                    {
                        delete[] temp;
                        return;
                    }
                    for (; i < length; ++i)
                    {
                        if (i == commandIndex)
                        {
                            temp[i] = ch;
                            continue;
                        }
                        temp[i] = data[j++];
                    }
                    temp[i] = '\0';
                    commandIndex += 1;
                }
            }

            String updatedData = new String(temp);
            UpdateOperand(m_tokenPosition, updatedData);
            displayExpressionToken.Token = updatedData;
            IsOperandUpdatedUsingViewModel = true;
            displayExpressionToken.CommandIndex = commandIndex;
        }

        bool IsOperator(Command cmdenum)
        {
            if ((cmdenum >= Command.Command0 && cmdenum <= Command.Command9) || (cmdenum == Command.CommandPNT) || (cmdenum == Command.CommandBACK)
                || (cmdenum == Command.CommandEXP) || (cmdenum == Command.CommandFE) || (cmdenum == Command.ModeBasic) || (cmdenum == Command.ModeProgrammer)
                || (cmdenum == Command.ModeScientific) || (cmdenum == Command.CommandINV) || (cmdenum == Command.CommandCENTR) || (cmdenum == Command.CommandDEG)
                || (cmdenum == Command.CommandRAD) || (cmdenum == Command.CommandGRAD)
                || ((cmdenum >= Command.CommandBINEDITSTART) && (cmdenum <= Command.CommandBINEDITEND)))
            {
                return false;
            }
            return true;
        }

        void OnButtonPressed(object parameter)
        {
            m_feedbackForButtonPress = CalculatorButtonPressedEventArgs.GetAuditoryFeedbackFromCommandParameter(parameter);
            NumbersAndOperatorsEnum numOpEnum = CalculatorButtonPressedEventArgs.GetOperationFromCommandParameter(parameter);
            Command cmdenum = ConvertToOperatorsEnum(numOpEnum);

            // // // // // // TraceLogger.GetInstance().UpdateFunctionUsage((int)numOpEnum);

            if (IsInError)
            {
                m_standardCalculatorManager.SendCommand(Command.CommandCLEAR);

                if (!IsRecoverableCommand((int)numOpEnum))
                {
                    return;
                }
            }

            if (IsEditingEnabled && numOpEnum != NumbersAndOperatorsEnum.IsScientificMode && numOpEnum != NumbersAndOperatorsEnum.IsStandardMode
                && numOpEnum != NumbersAndOperatorsEnum.IsProgrammerMode && numOpEnum != NumbersAndOperatorsEnum.FToE
                && (numOpEnum != NumbersAndOperatorsEnum.Degree) && (numOpEnum != NumbersAndOperatorsEnum.Radians) && (numOpEnum != NumbersAndOperatorsEnum.Grads))
            {
                if (!m_keyPressed)
                {
                    SaveEditedCommand(m_selectedExpressionToken.TokenPosition, cmdenum);
                }
            }
            else
            {
                if (numOpEnum == NumbersAndOperatorsEnum.IsStandardMode || numOpEnum == NumbersAndOperatorsEnum.IsScientificMode
                    || numOpEnum == NumbersAndOperatorsEnum.IsProgrammerMode)
                {
                    IsEditingEnabled = false;
                }
                if (numOpEnum == NumbersAndOperatorsEnum.Memory)
                {
                    OnMemoryButtonPressed();
                }
                else
                {
                    if (numOpEnum == NumbersAndOperatorsEnum.Clear || numOpEnum == NumbersAndOperatorsEnum.ClearEntry
                        || numOpEnum == NumbersAndOperatorsEnum.IsStandardMode || numOpEnum == NumbersAndOperatorsEnum.IsProgrammerMode)
                    {
                        // On Clear('C') the F-E button needs to be UnChecked if it in Checked state.
                        // Also, the Primary Display Value should not show in exponential format.
                        // Hence the check below to ensure parity with Desktop Calculator.
                        // Clear the FE mode if the switching to StandardMode, since 'C'/'CE' in StandardMode
                        // doesn't honor the FE button.
                        if (IsFToEChecked)
                        {
                            IsFToEChecked = false;
                        }
                    }
                    if (numOpEnum == NumbersAndOperatorsEnum.Degree || numOpEnum == NumbersAndOperatorsEnum.Radians || numOpEnum == NumbersAndOperatorsEnum.Grads)
                    {
                        m_CurrentAngleType = numOpEnum;
                    }
                    if ((cmdenum >= Command.Command0 && cmdenum <= Command.Command9) || (cmdenum == Command.CommandPNT) || (cmdenum == Command.CommandBACK)
                        || (cmdenum == Command.CommandEXP))
                    {
                        IsOperatorCommand = false;
                    }
                    else
                    {
                        IsOperatorCommand = true;
                    }

                    if (m_isLastOperationHistoryLoad
                        && ((numOpEnum != NumbersAndOperatorsEnum.Degree) && (numOpEnum != NumbersAndOperatorsEnum.Radians)
                            && (numOpEnum != NumbersAndOperatorsEnum.Grads)))
                    {
                        IsFToEEnabled = true;
                        m_isLastOperationHistoryLoad = false;
                    }

                    m_standardCalculatorManager.SendCommand(cmdenum);
                }
            }
        }

        int GetBitLengthType()
        {
            if (IsQwordEnabled)
            {
                return QwordType;
            }
            else if (IsDwordEnabled)
            {
                return DwordType;
            }
            else if (IsWordEnabled)
            {
                return WordType;
            }
            else
            {
                return ByteType;
            }
        }

        int GetNumberBase()
        {
            if (CurrentRadixType == HEX_RADIX)
            {
                return HexBase;
            }
            else if (CurrentRadixType == DEC_RADIX)
            {
                return DecBase;
            }
            else if (CurrentRadixType == OCT_RADIX)
            {
                return OctBase;
            }
            else
            {
                return BinBase;
            }
        }

        void OnCopyCommand(object parameter)
        {
            CopyPasteManager.CopyToClipboard(GetRawDisplayValue());

            String announcement = AppResourceProvider.GetInstance().GetResourceString(CalculatorResourceKeys.DisplayCopied);
            Announcement = CalculatorAnnouncement.GetDisplayCopiedAnnouncement(announcement);
        }

        void OnPasteCommand(object parameter)
        {
            ViewMode mode;
            int NumberBase = -1;
            int bitLengthType = -1;
            if (IsScientific)
            {
                mode = ViewMode.Scientific;
            }
            else if (IsProgrammer)
            {
                mode = ViewMode.Programmer;
                NumberBase = GetNumberBase();
                bitLengthType = GetBitLengthType();
            }
            else
            {
                mode = ViewMode.Standard;
            }
            // if there's nothing to copy early out
            if (IsEditingEnabled || !CopyPasteManager.HasStringToPaste())
            {
                return;
            }

            // Ensure that the paste happens on the UI thread
            CopyPasteManager.GetStringToPaste(mode, NavCategory.GetGroupType(mode), NumberBase, bitLengthType)
                .then([this, mode](String pastedString) { OnPaste(pastedString, mode); }, concurrency.task_continuation_context.use_current());
        }

        CalculationManager.Command ConvertToOperatorsEnum(NumbersAndOperatorsEnum operation)
        {
            return Command > (operation);
        }

        void OnPaste(String pastedString, ViewMode mode)
        {
            // If pastedString is invalid("NoOp") then display pasteError else process the string
            if (pastedString == string(CopyPasteManager.PasteErrorString))
            {
                this.DisplayPasteError();
                return;
            }

            // // // // // // TraceLogger.GetInstance().LogValidInputPasted(mode);
            bool isFirstLegalChar = true;
            m_standardCalculatorManager.SendCommand(Command.CommandCENTR);
            bool sendNegate = false;
            bool processedDigit = false;
            bool sentEquals = false;
            bool isPreviousOperator = false;

            List<bool> negateStack;

            // Iterate through each character pasted, and if it's valid, send it to the model.
            var it = pastedString.Begin();

            while (it != pastedString.End())
            {
                bool sendCommand = true;
                bool canSendNegate = false;

                NumbersAndOperatorsEnum mappedNumOp = MapCharacterToButtonId(*it, canSendNegate);

                if (mappedNumOp == NumbersAndOperatorsEnum.None)
                {
                    ++it;
                    continue;
                }

                if (isFirstLegalChar || isPreviousOperator)
                {
                    isFirstLegalChar = false;
                    isPreviousOperator = false;

                    // If the character is a - sign, send negate
                    // after sending the next legal character.  Send nothing now, or
                    // it will be ignored.
                    if (NumbersAndOperatorsEnum.Subtract == mappedNumOp)
                    {
                        sendNegate = true;
                        sendCommand = false;
                    }

                    // Support (+) sign pix
                    if (NumbersAndOperatorsEnum.Add == mappedNumOp)
                    {
                        sendCommand = false;
                    }
                }

                switch (mappedNumOp)
                {
                    // Opening parenthesis starts a new expression and pushes negation state onto the stack
                    case NumbersAndOperatorsEnum.OpenParenthesis:
                        negateStack.push_back(sendNegate);
                        sendNegate = false;
                        break;

                    // Closing parenthesis pops the negation state off the stack and sends it down to the calc engine
                    case NumbersAndOperatorsEnum.CloseParenthesis:
                        if (!negateStack.empty())
                        {
                            sendNegate = negateStack.back();
                            negateStack.pop_back();
                            canSendNegate = true;
                        }
                        else
                        {
                            // Don't send a closing parenthesis if a matching opening parenthesis hasn't been sent already
                            sendCommand = false;
                        }
                        break;

                    case NumbersAndOperatorsEnum.Zero:
                    case NumbersAndOperatorsEnum.One:
                    case NumbersAndOperatorsEnum.Two:
                    case NumbersAndOperatorsEnum.Three:
                    case NumbersAndOperatorsEnum.Four:
                    case NumbersAndOperatorsEnum.Five:
                    case NumbersAndOperatorsEnum.Six:
                    case NumbersAndOperatorsEnum.Seven:
                    case NumbersAndOperatorsEnum.Eight:
                    case NumbersAndOperatorsEnum.Nine:
                        processedDigit = true;
                        break;

                    case NumbersAndOperatorsEnum.Add:
                    case NumbersAndOperatorsEnum.Subtract:
                    case NumbersAndOperatorsEnum.Multiply:
                    case NumbersAndOperatorsEnum.Divide:
                        isPreviousOperator = true;
                        break;
                }

                if (sendCommand)
                {
                    sentEquals = (mappedNumOp == NumbersAndOperatorsEnum.Equals);
                    Command cmdenum = ConvertToOperatorsEnum(mappedNumOp);
                    m_standardCalculatorManager.SendCommand(cmdenum);

                    // The CalcEngine state machine won't allow the negate command to be sent before any
                    // other digits, so instead a flag is set and the command is sent after the first appropriate
                    // command.
                    if (sendNegate)
                    {
                        if (canSendNegate)
                        {
                            Command cmdNegate = ConvertToOperatorsEnum(NumbersAndOperatorsEnum.Negate);
                            m_standardCalculatorManager.SendCommand(cmdNegate);
                        }

                        // Can't send negate on a leading zero, so wait until the appropriate time to send it.
                        if (NumbersAndOperatorsEnum.Zero != mappedNumOp && NumbersAndOperatorsEnum.Decimal != mappedNumOp)
                        {
                            sendNegate = false;
                        }
                    }
                }

                // Handle exponent and exponent sign (...e+... or ...e-...)
                if (mappedNumOp == NumbersAndOperatorsEnum.Exp)
                {
                    ++it;
                    if (!(MapCharacterToButtonId(*it, canSendNegate) == NumbersAndOperatorsEnum.Add))
                    {
                        Command cmdNegate = ConvertToOperatorsEnum(NumbersAndOperatorsEnum.Negate);
                        m_standardCalculatorManager.SendCommand(cmdNegate);
                    }
                }

                ++it;
            }
        }

        void OnClearMemoryCommand(object parameter)
        {
            m_standardCalculatorManager.MemorizedNumberClearAll();

            int windowId = Utils.GetWindowId();
            // // // // // // TraceLogger.GetInstance().LogMemoryClearAll(windowId);

            String announcement = LocalizationStringUtil.GetLocalizedNarratorAnnouncement(CalculatorResourceKeys.MemoryCleared, m_localizedMemoryCleared);
            Announcement = CalculatorAnnouncement.GetMemoryClearedAnnouncement(announcement);
        }

        void OnPinUnpinCommand(object parameter)
        {
            SetViewPinnedState(!IsViewPinned());
        }

        bool IsViewPinned()
        {
            return m_IsCurrentViewPinned;
        }

        void SetViewPinnedState(bool pinned)
        {
            IsCurrentViewPinned = pinned;
        }

        NumbersAndOperatorsEnum MapCharacterToButtonId(char ch,  bool canSendNegate)
        {
            NumbersAndOperatorsEnum mappedValue = NumbersAndOperatorsEnum.None;
            canSendNegate = false;

            switch (ch)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    mappedValue = NumbersAndOperatorsEnum.Zero + (NumbersAndOperatorsEnum)(ch - '0');
                    canSendNegate = true;
                    break;

                case '*':
                    mappedValue = NumbersAndOperatorsEnum.Multiply;
                    break;

                case '+':
                    mappedValue = NumbersAndOperatorsEnum.Add;
                    break;

                case '-':
                    mappedValue = NumbersAndOperatorsEnum.Subtract;
                    break;

                case '/':
                    mappedValue = NumbersAndOperatorsEnum.Divide;
                    break;

                case '=':
                    mappedValue = NumbersAndOperatorsEnum.Equals;
                    break;

                case '(':
                    mappedValue = NumbersAndOperatorsEnum.OpenParenthesis;
                    break;

                case ')':
                    mappedValue = NumbersAndOperatorsEnum.CloseParenthesis;
                    break;

                case 'a':
                case 'A':
                    mappedValue = NumbersAndOperatorsEnum.A;
                    break;
                case 'b':
                case 'B':
                    mappedValue = NumbersAndOperatorsEnum.B;
                    break;
                case 'c':
                case 'C':
                    mappedValue = NumbersAndOperatorsEnum.C;
                    break;
                case 'd':
                case 'D':
                    mappedValue = NumbersAndOperatorsEnum.D;
                    break;
                case 'e':
                case 'E':
                    // Only allow scientific notation in scientific mode
                    if (IsProgrammer)
                    {
                        mappedValue = NumbersAndOperatorsEnum.E;
                    }
                    else
                    {
                        mappedValue = NumbersAndOperatorsEnum.Exp;
                    }
                    break;
                case 'f':
                case 'F':
                    mappedValue = NumbersAndOperatorsEnum.F;
                    break;
                default:
                    // For the decimalSeparator, we need to respect the user setting.
                    if (ch == m_decimalSeparator)
                    {
                        mappedValue = NumbersAndOperatorsEnum.Decimal;
                    }
                    break;
            }

            if (mappedValue == NumbersAndOperatorsEnum.None)
            {
                if (LocalizationSettings.GetInstance().IsLocalizedDigit(ch))
                {
                    mappedValue =
                        NumbersAndOperatorsEnum.Zero + (NumbersAndOperatorsEnum)(ch - LocalizationSettings.GetInstance().GetDigitSymbolFromEnUsDigit('0'));
                    canSendNegate = true;
                }
            }

            // Negate cannot be sent for leading zeroes
            if (NumbersAndOperatorsEnum.Zero == mappedValue)
            {
                canSendNegate = false;
            }

            return mappedValue;
        }

        void OnMemoryButtonPressed()
        {
            m_standardCalculatorManager.MemorizeNumber();

            int windowId = Utils.GetWindowId();
            // // // // // // TraceLogger.GetInstance().InsertIntoMemoryMap(windowId, IsStandard, IsScientific, IsProgrammer);

            String announcement = LocalizationStringUtil.GetLocalizedNarratorAnnouncement(
                CalculatorResourceKeys.MemorySave, m_localizedMemorySavedAutomationFormat, m_DisplayValue.Data());

            Announcement = CalculatorAnnouncement.GetMemoryItemAddedAnnouncement(announcement);
        }

        void OnMemoryItemChanged(uint indexOfMemory)
        {
            if (indexOfMemory < MemorizedNumbers.Size)
            {
                MemoryItemViewModel memSlot = MemorizedNumbers.GetAt(indexOfMemory);
                String localizedValue = memSlot.Value;

                string localizedIndex = to_string(indexOfMemory + 1);
                LocalizationSettings.GetInstance().LocalizeDisplayValue(&localizedIndex);

                String announcement = LocalizationStringUtil.GetLocalizedNarratorAnnouncement(
                    CalculatorResourceKeys.MemoryItemChanged, m_localizedMemoryItemChangedAutomationFormat, localizedIndex, localizedValue.Data());

                Announcement = CalculatorAnnouncement.GetMemoryItemChangedAnnouncement(announcement);
            }
        }

        void OnMemoryItemPressed(object memoryItemPosition)
        {
            if (MemorizedNumbers && MemorizedNumbers.Size > 0)
            {
                var boxedPosition = safe_cast < Box<int>(memoryItemPosition);
                m_standardCalculatorManager.MemorizedNumberLoad(boxedPosition.Value);
                HideMemoryClicked();
                int windowId = Utils.GetWindowId();
                // // // // // // TraceLogger.GetInstance().LogMemoryUsed(windowId, boxedPosition.Value, IsStandard, IsScientific, IsProgrammer, MemorizedNumbers.Size);
            }
        }

        void OnMemoryAdd(object memoryItemPosition)
        {
            // M+ will add display to memorylist if memory list is empty.
            int windowId = Utils.GetWindowId();

            if (MemorizedNumbers)
            {
                var boxedPosition = safe_cast < Box<int>(memoryItemPosition);
                if (MemorizedNumbers.Size > 0)
                {
                    // // // // // // TraceLogger.GetInstance().LogMemoryUsed(windowId, boxedPosition.Value, IsStandard, IsScientific, IsProgrammer, MemorizedNumbers.Size);
                    // // // // // // TraceLogger.GetInstance().UpdateMemoryMap(windowId, boxedPosition.Value, IsStandard, IsScientific, IsProgrammer);
                }
                else
                {
                    // // // // // // TraceLogger.GetInstance().InsertIntoMemoryMap(windowId, IsStandard, IsScientific, IsProgrammer);
                }
                m_standardCalculatorManager.MemorizedNumberAdd(boxedPosition.Value);
            }
        }

        void OnMemorySubtract(object memoryItemPosition)
        {
            int windowId = Utils.GetWindowId();

            // M- will add negative of displayed number to memorylist if memory list is empty.
            if (MemorizedNumbers)
            {
                var boxedPosition = safe_cast < Box<int>(memoryItemPosition);
                if (MemorizedNumbers.Size > 0)
                {
                    // // // // // // TraceLogger.GetInstance().LogMemoryUsed(windowId, boxedPosition.Value, IsStandard, IsScientific, IsProgrammer, MemorizedNumbers.Size);
                    // // // // // // TraceLogger.GetInstance().UpdateMemoryMap(windowId, boxedPosition.Value, IsStandard, IsScientific, IsProgrammer);
                }
                else
                {
                    // // // // // // TraceLogger.GetInstance().InsertIntoMemoryMap(windowId, IsStandard, IsScientific, IsProgrammer);
                }
                m_standardCalculatorManager.MemorizedNumberSubtract(boxedPosition.Value);
            }
        }

        void OnMemoryClear(object memoryItemPosition)
        {
            if (MemorizedNumbers && MemorizedNumbers.Size > 0)
            {
                int windowId = Utils.GetWindowId();
                var boxedPosition = safe_cast < Box<int>(memoryItemPosition);

                if (boxedPosition.Value >= 0)
                {
                    uint unsignedPosition = uint > (boxedPosition.Value);
                    m_standardCalculatorManager.MemorizedNumberClear(unsignedPosition);

                    MemorizedNumbers.RemoveAt(unsignedPosition);
                    for (uint i = 0; i < MemorizedNumbers.Size; i++)
                    {
                        MemorizedNumbers.GetAt(i).Position = i;
                    }

                    if (MemorizedNumbers.Size == 0)
                    {
                        IsMemoryEmpty = true;
                    }

                    // // // // // // TraceLogger.GetInstance().LogMemoryUsed(windowId, boxedPosition.Value, IsStandard, IsScientific, IsProgrammer, MemorizedNumbers.Size);
                    // // // // // // TraceLogger.GetInstance().DeleteFromMemoryMap(windowId, boxedPosition.Value);

                    string localizedIndex = to_string(boxedPosition.Value + 1);
                    LocalizationSettings.GetInstance().LocalizeDisplayValue(&localizedIndex);

                    String announcement = LocalizationStringUtil.GetLocalizedNarratorAnnouncement(
                        CalculatorResourceKeys.MemoryItemCleared, m_localizedMemoryItemClearedAutomationFormat, localizedIndex);

                    Announcement = CalculatorAnnouncement.GetMemoryClearedAnnouncement(announcement);
                }
            }
        }

        Array<unsigned char> Serialize()
        {
            DataWriter writer = new DataWriter();
            writer.Writeuint((uint)(m_CurrentAngleType));
            writer.WriteBoolean(IsFToEChecked);
            writer.WriteBoolean(IsCurrentViewPinned);
            writer.Writeuint((uint)(m_standardCalculatorManager.SerializeSavedDegreeMode()));

            // Serialize Memory
            List<long> serializedMemory;
            serializedMemory = m_standardCalculatorManager.GetSerializedMemory();
            int lengthOfSerializedMemory = serializedMemory.size();
            writer.Writeuint((uint)(lengthOfSerializedMemory));
            for (var data : serializedMemory)
            {
                writer.WriteInt32(data);
            }

            // Serialize Primary Display
            List<long> serializedPrimaryDisplay = m_standardCalculatorManager.GetSerializedPrimaryDisplay();
            writer.Writeuint((uint)(serializedPrimaryDisplay.size()));
            for (var data : serializedPrimaryDisplay)
            {
                writer.WriteInt32(data);
            }

            // For ProgrammerMode
            writer.Writeuint((uint)(CurrentRadixType));

            // Serialize commands of calculator manager
            vector < unsigned char> serializedCommand = m_standardCalculatorManager.SerializeCommands();
            writer.Writeuint((uint)(serializedCommand.size()));
            writer.WriteBytes(new Array<unsigned char>(serializedCommand.data(), (uint)(serializedCommand.size())));

            if (IsInError)
            {
                Utils.SerializeCommandsAndTokens(m_tokens, m_commands, writer);
            }

            // Convert viewmodel data in writer to bytes
            IBuffer buffer = writer.DetachBuffer();
            DataReader reader = DataReader.FromBuffer(buffer);
            Platform.Array < unsigned char> viewModelDataAsBytes = new Array<unsigned char>(buffer.Length);
            reader.ReadBytes(viewModelDataAsBytes);

            // Return byte array
            return viewModelDataAsBytes;
        }

        void Deserialize(char[] state)
        {
            // Read byte array into a buffer
            DataWriter writer = new DataWriter();
            writer.WriteBytes(state);
            IBuffer buffer = writer.DetachBuffer();

            // Read view model data
            if (buffer.Length != 0)
            {
                DataReader reader = DataReader.FromBuffer(buffer);
                m_CurrentAngleType = ConvertIntegerToNumbersAndOperatorsEnum(reader.Readuint());

                IsFToEChecked = reader.ReadBoolean();
                IsCurrentViewPinned = reader.ReadBoolean();
                Command serializedDegreeMode = (Command)(reader.Readuint());

                m_standardCalculatorManager.SendCommand(serializedDegreeMode);

                // Deserialize Memory
                uint memoryDataLength = reader.Readuint();
                List<long> serializedMemory;
                for (uint i = 0; i < memoryDataLength; i++)
                {
                    serializedMemory.push_back(reader.ReadInt32());
                }
                m_standardCalculatorManager.DeSerializeMemory(serializedMemory);

                // Serialize Primary Display
                uint serializedPrimaryDisplayLength = reader.Readuint();
                List<long> serializedPrimaryDisplay;
                for (uint i = 0; i < serializedPrimaryDisplayLength; i++)
                {
                    serializedPrimaryDisplay.push_back(reader.ReadInt32());
                }
                m_standardCalculatorManager.DeSerializePrimaryDisplay(serializedPrimaryDisplay);

                CurrentRadixType = reader.Readuint();
                // Read command data and Deserialize
                uint modeldatalength = reader.Readuint();
                Array < unsigned char> modelDataAsBytes = new Array<unsigned char>(modeldatalength);
                reader.ReadBytes(modelDataAsBytes);
                m_standardCalculatorManager.DeSerializeCommands(vector < unsigned char > (modelDataAsBytes.begin(), modelDataAsBytes.end()));

                // After recalculation. If there is an error then
                // IsInError should be set synchronously.
                if (IsInError)
                {
                    CalculatorList<IExpressionCommand> commandVector = Utils.DeserializeCommands(reader);
                    CalculatorList<pair<string, int>> tokenVector = Utils.DeserializeTokens(reader);
                    SetExpressionDisplay(tokenVector, commandVector);
                }
            }
        }

        void OnPropertyChanged(String propertyname)
        {
            if (propertyname == IsScientificPropertyName)
            {
                if (IsScientific)
                {
                    OnButtonPressed(NumbersAndOperatorsEnum.IsScientificMode);
                }
            }
            else if (propertyname == IsProgrammerPropertyName)
            {
                if (IsProgrammer)
                {
                    OnButtonPressed(NumbersAndOperatorsEnum.IsProgrammerMode);
                }
            }
            else if (propertyname == IsStandardPropertyName)
            {
                if (IsStandard)
                {
                    OnButtonPressed(NumbersAndOperatorsEnum.IsStandardMode);
                }
            }
            else if (propertyname == DisplayValuePropertyName)
            {
                RaisePropertyChanged(CalculationResultAutomationNamePropertyName);
                Announcement = GetDisplayUpdatedNarratorAnnouncement();
            }
        }

        void SetCalculatorType(ViewMode targetState)
        {
            // Reset error state so that commands caused by the mode change are still
            // sent if calc is currently in error state.
            IsInError = false;

            // Setting one of these properties to true will set the others to false.
            switch (targetState)
            {
                case ViewMode.Standard:
                    IsStandard = true;
                    ResetDisplay();
                    SetPrecision(StandardModePrecision);
                    UpdateMaxIntDigits();
                    break;

                case ViewMode.Scientific:
                    IsScientific = true;
                    ResetDisplay();
                    SetPrecision(ScientificModePrecision);
                    break;

                case ViewMode.Programmer:
                    IsProgrammer = true;
                    ResetDisplay();
                    SetPrecision(ProgrammerModePrecision);
                    break;
            }
        }

        string GetRawDisplayValue()
        {
            string rawValue;

            LocalizationSettings.GetInstance().RemoveGroupSeparators(DisplayValue.Data(), DisplayValue.Length(), &rawValue);

            return new Platform.String(rawValue);
        }

        // Given a format string, returns a string with the input display value inserted.
        //     'format' is a localized string containing a %1 formatting mark where the display value should be inserted.
        //     'displayValue' is a localized string containing a numerical value to be displayed to the user.
        String GetLocalizedStringFormat(String format, String displayValue)
        {
            String localizedString = new String(LocalizationStringUtil.GetLocalizedString(format.Data(), displayValue.Data()));
            return localizedString;
        }

        void ResetDisplay()
        {
            AreHEXButtonsEnabled = false;
            CurrentRadixType = (int)RADIX_TYPE.DEC_RADIX;
            m_standardCalculatorManager.SetRadix(DEC_RADIX);
            ProgModeRadixChange();
        }

        void SetPrecision(int precision)
        {
            m_standardCalculatorManager.SetPrecision(precision);
        }

        void SwitchProgrammerModeBase(RADIX_TYPE radixType)
        {
            if (IsInError)
            {
                m_standardCalculatorManager.SendCommand(Command.CommandCLEAR);
            }

            AreHEXButtonsEnabled = (radixType == RADIX_TYPE.HEX_RADIX);
            CurrentRadixType = (int)radixType;
            m_standardCalculatorManager.SetRadix(radixType);
            ProgModeRadixChange();
        }

        void SetMemorizedNumbersString()
        {
            m_standardCalculatorManager.SetMemorizedNumbersString();
        }

        ANGLE_TYPE GetAngleTypeFromCommand(Command command)
        {
            switch (command)
            {
                case Command.CommandDEG:
                    return ANGLE_DEG;
                case Command.CommandRAD:
                    return ANGLE_RAD;
                case Command.CommandGRAD:
                    return ANGLE_GRAD;
                default:
                    throw new Exception(E_FAIL, "Invalid command type");
            }
        }

        void SaveEditedCommand(uint tokenPosition, Command command)
        {
            pair<string, int> token;
            bool handleOperand = false;
            int nOpCode = (int)(command);
            string updatedToken = "";

            IExpressionCommand tokenCommand;
            IFTPlatformException(m_tokens.GetAt(tokenPosition, &token));

            uint tokenCommandIndex = token.second;
            IFTPlatformException(m_commands.GetAt(tokenCommandIndex, &tokenCommand));

            if (IsUnaryOp(nOpCode) && command != Command.CommandSIGN)
            {
                int angleCmd = (int)(m_standardCalculatorManager.GetCurrentDegreeMode());
                ANGLE_TYPE angleType = GetAngleTypeFromCommand((Command)(angleCmd));

                if (IsTrigOp(nOpCode))
                {
                    IUnaryCommand spUnaryCommand = (IUnaryCommand)(tokenCommand);
                    spUnaryCommand.SetCommands(angleCmd, nOpCode);
                }
                else
                {
                    IUnaryCommand spUnaryCommand = (IUnaryCommand)(tokenCommand);
                    spUnaryCommand.SetCommand(nOpCode);
                }

                switch (nOpCode)
                {
                    case (int)(Command.CommandASIN):
                        updatedToken = CCalcEngine.OpCodeToUnaryString((int)(Command.CommandSIN), true, angleType);
                        break;
                    case (int)(Command.CommandACOS):
                        updatedToken = CCalcEngine.OpCodeToUnaryString((int)(Command.CommandCOS), true, angleType);
                        break;
                    case (int)(Command.CommandATAN):
                        updatedToken = CCalcEngine.OpCodeToUnaryString((int)(Command.CommandTAN), true, angleType);
                        break;
                    case (int)(Command.CommandASINH):
                        updatedToken = CCalcEngine.OpCodeToUnaryString((int)(Command.CommandSINH), true, angleType);
                        break;
                    case (int)(Command.CommandACOSH):
                        updatedToken = CCalcEngine.OpCodeToUnaryString((int)(Command.CommandCOSH), true, angleType);
                        break;
                    case (int)(Command.CommandATANH):
                        updatedToken = CCalcEngine.OpCodeToUnaryString((int)(Command.CommandTANH), true, angleType);
                        break;
                    case (int)(Command.CommandPOWE):
                        updatedToken = CCalcEngine.OpCodeToUnaryString((int)(Command.CommandLN), true, angleType);
                        break;
                    default:
                        updatedToken = CCalcEngine.OpCodeToUnaryString(nOpCode, false, angleType);
                }
                if ((token.first.length() > 0) && (token.first[token.first.length() - 1] == '('))
                {
                    string chOpenBrace = "(";
                    updatedToken.append(chOpenBrace);
                }
            }
            else if (IsBinOp(nOpCode))
            {
                IBinaryCommand spBinaryCommand = (IBinaryCommand)(tokenCommand);
                spBinaryCommand.SetCommand(nOpCode);
                updatedToken = CCalcEngine.OpCodeToString(nOpCode);
            }
            else if (IsOpnd(nOpCode) || command == Command.CommandBACK)
            {
                HandleUpdatedOperandData(command);
                handleOperand = true;
            }
            else if (command == Command.CommandSIGN)
            {
                if (tokenCommand.GetCommandType() == CommandType.UnaryCommand)
                {
                    IExpressionCommand spSignCommand = new CUnaryCommand(nOpCode);
                    IFTPlatformException(m_commands.InsertAt(tokenCommandIndex + 1, spSignCommand));
                }
                else
                {
                    IOpndCommand spOpndCommand = (IOpndCommand)(tokenCommand);
                    spOpndCommand.ToggleSign();
                    updatedToken = spOpndCommand.GetToken(m_standardCalculatorManager.DecimalSeparator());
                }
                IsOperandUpdatedUsingViewModel = true;
            }

            if (!handleOperand)
            {
                IFTPlatformException(m_commands.SetAt(tokenCommandIndex, tokenCommand));

                pair<string, int> selectedToken;
                IFTPlatformException(m_tokens.GetAt(tokenPosition, &selectedToken));
                selectedToken.first = updatedToken;
                IFTPlatformException(m_tokens.SetAt(tokenPosition, selectedToken));

                DisplayExpressionToken displayExpressionToken = ExpressionTokens.GetAt(tokenPosition);
                displayExpressionToken.Token = new Platform.String(updatedToken);

                // Special casing
                if (command == Command.CommandSIGN && tokenCommand.GetCommandType() == CommandType.UnaryCommand)
                {
                    IsEditingEnabled = false;
                    Recalculate();
                }
            }
        }

        void Recalculate(bool fromHistory)
        {
            // Recalculate
            Command currentDegreeMode = m_standardCalculatorManager.GetCurrentDegreeMode();
            CalculatorList<IExpressionCommand> savedCommands = new CalculatorList<IExpressionCommand>();

            List<int> currentCommands;
            uint commandListCount;
            m_commands.GetSize(&commandListCount);
            for (uint i = 0; i < commandListCount; i++)
            {
                IExpressionCommand command;
                IFTPlatformException(m_commands.GetAt(i, &command));

                savedCommands.Append(command);
                CommandType commandType = command.GetCommandType();

                if (commandType == CommandType.UnaryCommand)
                {
                    IUnaryCommand spCommand = (IUnaryCommand)(command);
                    CalculatorList<int> unaryCommands = spCommand.GetCommands();
                    uint unaryCommandCount;
                    unaryCommands.GetSize(&unaryCommandCount);

                    int nUCode;
                    for (uint j = 0; j < unaryCommandCount; ++j)
                    {
                        IFTPlatformException(unaryCommands.GetAt(j, &nUCode));
                        currentCommands.push_back(nUCode);
                    }
                }

                if (commandType == CommandType.BinaryCommand)
                {
                    IBinaryCommand spCommand = (IBinaryCommand)(command);
                    currentCommands.push_back(spCommand.GetCommand());
                }

                if (commandType == CommandType.Parentheses)
                {
                    IParenthesisCommand spCommand = (IParenthesisCommand)(command);
                    currentCommands.push_back(spCommand.GetCommand());
                }

                if (commandType == CommandType.OperandCommand)
                {
                    IOpndCommand spCommand = (IOpndCommand)(command);
                    CalculatorList<int> opndCommands = spCommand.GetCommands();
                    uint opndCommandCount;
                    opndCommands.GetSize(&opndCommandCount);
                    bool fNeedIDCSign = spCommand.IsNegative();

                    int nOCode;
                    for (uint j = 0; j < opndCommandCount; ++j)
                    {
                        IFTPlatformException(opndCommands.GetAt(j, &nOCode));
                        currentCommands.push_back(nOCode);

                        if (fNeedIDCSign && nOCode != IDC_0)
                        {
                            currentCommands.push_back((int)(CalculationManager.Command.CommandSIGN));
                            fNeedIDCSign = false;
                        }
                    }
                }
            }
            CalculatorList<pair<string, int>> savedTokens = new CalculatorList<pair<string, int>>();

            uint tokenCount;
            IFTPlatformException(m_tokens.GetSize(&tokenCount));

            for (uint i = 0; i < tokenCount; ++i)
            {
                pair<string, int> currentToken;
                IFTPlatformException(m_tokens.GetAt(i, &currentToken));
                savedTokens.Append(currentToken);
            }

            m_standardCalculatorManager.Reset(false);
            if (IsScientific)
            {
                m_standardCalculatorManager.SendCommand(Command.ModeScientific);
            }

            if (IsFToEChecked)
            {
                m_standardCalculatorManager.SendCommand(Command.CommandFE);
            }

            m_standardCalculatorManager.SendCommand(currentDegreeMode);
            int currentCommandsSize = currentCommands.size();
            for (int i = 0; i < currentCommandsSize; i++)
            {
                m_standardCalculatorManager.SendCommand((CalculationManager.Command)(currentCommands[i]));
            }

            if (fromHistory) // This is for the cases where the expression is loaded from history
            {
                // To maintain F-E state of the engine, as the last operand hasn't reached engine by now
                m_standardCalculatorManager.SendCommand(Command.CommandFE);
                m_standardCalculatorManager.SendCommand(Command.CommandFE);
            }

            // After recalculation. If there is an error then
            // IsInError should be set synchronously.
            if (IsInError)
            {
                SetExpressionDisplay(savedTokens, savedCommands);
            }
        }

        CommandType GetSelectedTokenType(uint tokenPosition)
        {
            pair<string, int> token;
            IExpressionCommand tokenCommand;
            IFTPlatformException(m_tokens.GetAt(tokenPosition, &token));

            uint tokenCommandIndex = token.second;
            IFTPlatformException(m_commands.GetAt(tokenCommandIndex, &tokenCommand));

            return tokenCommand.GetCommandType();
        }

        bool IsOpnd(int nOpCode)
        {
            static Command opnd[] = { Command.Command0, Command.Command1, Command.Command2, Command.Command3, Command.Command4,  Command.Command5,
                              Command.Command6, Command.Command7, Command.Command8, Command.Command9, Command.CommandPNT };

            for (uint i = 0; i < size(opnd); i++)
            {
                if (nOpCode == (int)(opnd[i]))
                {
                    return true;
                }
            }
            return false;
        }

        bool IsUnaryOp(int nOpCode)
        {
            static Command unaryOp[] = { Command.CommandSQRT,  Command.CommandFAC,  Command.CommandSQR,   Command.CommandLOG,
                                 Command.CommandPOW10, Command.CommandPOWE, Command.CommandLN,    Command.CommandREC,
                                 Command.CommandSIGN,  Command.CommandSINH, Command.CommandASINH, Command.CommandCOSH,
                                 Command.CommandACOSH, Command.CommandTANH, Command.CommandATANH, Command.CommandCUB };

            for (uint i = 0; i < size(unaryOp); i++)
            {
                if (nOpCode == (int)(unaryOp[i]))
                {
                    return true;
                }
            }

            if (IsTrigOp(nOpCode))
            {
                return true;
            }

            return false;
        }

        bool IsTrigOp(int nOpCode)
        {
            static Command trigOp[] = {
        Command.CommandSIN, Command.CommandCOS, Command.CommandTAN, Command.CommandASIN, Command.CommandACOS, Command.CommandATAN
    };

            for (uint i = 0; i < size(trigOp); i++)
            {
                if (nOpCode == (int)(trigOp[i]))
                {
                    return true;
                }
            }
            return false;
        }

        bool IsBinOp(int nOpCode)
        {
            static Command binOp[] = { Command.CommandADD, Command.CommandSUB,  Command.CommandMUL, Command.CommandDIV,
                               Command.CommandEXP, Command.CommandROOT, Command.CommandMOD, Command.CommandPWR };

            for (uint i = 0; i < size(binOp); i++)
            {
                if (nOpCode == (int)(binOp[i]))
                {
                    return true;
                }
            }
            return false;
        }

        bool IsRecoverableCommand(int nOpCode)
        {
            if (IsOpnd(nOpCode))
            {
                return true;
            }

            // Programmer mode, bit flipping
            int minBinPos = (int)(Command.CommandBINEDITSTART);
            int maxBinPos = (int)(Command.CommandBINEDITEND);
            if (minBinPos <= nOpCode && nOpCode <= maxBinPos)
            {
                return true;
            }

            static Command recoverableCommands[] = { Command.CommandA, Command.CommandB, Command.CommandC, Command.CommandD, Command.CommandE, Command.CommandF };

            for (uint i = 0; i < size(recoverableCommands); i++)
            {
                if (nOpCode == (int)(recoverableCommands[i]))
                {
                    return true;
                }
            }
            return false;
        }

        int LengthWithoutPadding(string str)
        {
            int count = 0;
            for (int i = 0; i < str.length(); i++)
            {
                if (str[i] != ' ')
                {
                    count++;
                }
            }
            return count;
        }

        string AddPadding(string binaryString)
        {
            if (LocalizationSettings.GetInstance().GetEnglishValueFromLocalizedDigits(binaryString) == "0")
            {
                return binaryString;
            }
            int pad = 4 - LengthWithoutPadding(binaryString) % 4;
            if (pad == 4)
            {
                pad = 0;
            }
            string padString = "";
            for (int i = 0; i < pad; i++)
            {
                padString += "0";
            }
            return padString + binaryString;
        }

        void UpdateProgrammerPanelDisplay()
        {
            string hexDisplayString;
            string decimalDisplayString;
            string octalDisplayString;
            string binaryDisplayString;
            if (!IsInError)
            {
                // we want the precision to be set to maximum value so that the varconversions result as desired
                int precision = 64;
                if (m_standardCalculatorManager.GetResultForRadix(16, precision) == "")
                {
                    hexDisplayString = DisplayValue;
                    decimalDisplayString = DisplayValue;
                    octalDisplayString = DisplayValue;
                    binaryDisplayString = DisplayValue;
                }
                else
                {
                    hexDisplayString = m_standardCalculatorManager.GetResultForRadix(16, precision);
                    decimalDisplayString = m_standardCalculatorManager.GetResultForRadix(10, precision);
                    octalDisplayString = m_standardCalculatorManager.GetResultForRadix(8, precision);
                    binaryDisplayString = m_standardCalculatorManager.GetResultForRadix(2, precision);
                }
            }
            var localizer = LocalizationSettings.GetInstance();
            binaryDisplayString = AddPadding(binaryDisplayString);

            localizer.LocalizeDisplayValue(hexDisplayString);
            localizer.LocalizeDisplayValue(decimalDisplayString);
            localizer.LocalizeDisplayValue(octalDisplayString);
            localizer.LocalizeDisplayValue(binaryDisplayString);

            HexDisplayValue = Utils.LRO + new Platform.String(hexDisplayString) + Utils.PDF;
            DecimalDisplayValue = Utils.LRO + new Platform.String(decimalDisplayString) + Utils.PDF;
            OctalDisplayValue = Utils.LRO + new Platform.String(octalDisplayString) + Utils.PDF;
            BinaryDisplayValue = Utils.LRO + new Platform.String(binaryDisplayString) + Utils.PDF;
            HexDisplayValue_AutomationName = GetLocalizedStringFormat(m_localizedHexaDecimalAutomationFormat, GetNarratorStringReadRawNumbers(HexDisplayValue));
            DecDisplayValue_AutomationName = GetLocalizedStringFormat(m_localizedDecimalAutomationFormat, DecimalDisplayValue);
            OctDisplayValue_AutomationName = GetLocalizedStringFormat(m_localizedOctalAutomationFormat, GetNarratorStringReadRawNumbers(OctalDisplayValue));
            BinDisplayValue_AutomationName = GetLocalizedStringFormat(m_localizedBinaryAutomationFormat, GetNarratorStringReadRawNumbers(BinaryDisplayValue));
        }

        void SwitchAngleType(NumbersAndOperatorsEnum num)
        {
            OnButtonPressed(num);
        }

        NumbersAndOperatorsEnum ConvertIntegerToNumbersAndOperatorsEnum(uint parameter)
        {
            NumbersAndOperatorsEnum angletype;
            switch (parameter)
            {
                case 321:
                    angletype = NumbersAndOperatorsEnum.Degree;
                    break;
                case 322:
                    angletype = NumbersAndOperatorsEnum.Radians;
                    break;
                case 323:
                    angletype = NumbersAndOperatorsEnum.Grads;
                    break;
                default:
                    angletype = NumbersAndOperatorsEnum.Degree;
            };
            return angletype;
        }

        void UpdateOperand(int pos, String text)
        {
            pair<string, int> p;
            m_tokens.GetAt(pos, &p);

            String englishString = LocalizationSettings.GetInstance().GetEnglishValueFromLocalizedDigits(text.Data());
            p.first = englishString.Data();

            int commandPos = p.second;
            IExpressionCommand exprCmd;
            m_commands.GetAt(commandPos, &exprCmd);
            var operandCommand = std.(IOpndCommand)(exprCmd);

            if (operandCommand != null)
            {
                CalculatorList<int> commands = new CalculatorList<int>();
                int length = p.first.length();
                if (length > 0)
                {
                    int num = 0;
                    for (uint i = 0; i < length; ++i)
                    {
                        if (p.first[i] == '.')
                        {
                            num = (int)(Command.CommandPNT);
                        }
                        else if (p.first[i] == 'e')
                        {
                            num = (int)(Command.CommandEXP);
                        }
                        else if (p.first[i] == '-')
                        {
                            num = (int)(Command.CommandSIGN);

                            if (i == 0)
                            {
                                IOpndCommand spOpndCommand = (IOpndCommand)(exprCmd);
                                if (!spOpndCommand.IsNegative())
                                {
                                    spOpndCommand.ToggleSign();
                                }
                                continue;
                            }
                        }
                        else
                        {
                            num = (int)(p.first[i]) - ASCII_0;
                            num += IDC_0;
                            if (num == (int)(Command.CommandMPLUS))
                            {
                                continue;
                            }
                        }
                        commands.Append(num);
                    }
                }
                else
                {
                    commands.Append(0);
                }
                operandCommand.SetCommands(commands);
            }
        }

        void UpdatecommandsInRecordingMode()
        {
            vector < unsigned char> savedCommands = m_standardCalculatorManager.GetSavedCommands();
            CalculatorList<int> commands = new CalculatorList<int>();
            bool isDecimal = false;
            bool isNegative = false;
            bool isExpMode = false;
            bool ePlusMode = false;
            bool eMinusMode = false;

            int num = 0;
            Command val;
            for (uint i = 0; i < savedCommands.size(); ++i)
            {
                val = Command > (savedCommands[i]);
                num = (int)(val);
                if (val == Command.CommandSIGN)
                {
                    isNegative = true;
                    continue;
                }
                else if ((val >= Command.Command0 && val <= Command.Command9))
                {
                }
                else if (val == Command.CommandPNT)
                {
                    isDecimal = true;
                }
                else if (val == Command.CommandEXP)
                {
                    isExpMode = true;
                }
                else if (isExpMode && !ePlusMode && (val == Command.CommandMPLUS))
                {
                    ePlusMode = true;
                    continue;
                }
                else if (isExpMode && !eMinusMode && (val == Command.CommandMMINUS))
                {
                    eMinusMode = true;
                    continue;
                }
                else
                {
                    // Reset all vars
                    isDecimal = false;
                    isNegative = false;
                    isExpMode = false;
                    ePlusMode = false;
                    eMinusMode = false;
                    commands.Clear();
                    continue;
                }
                commands.Append(num);
            }

            uint size = 0;
            commands.GetSize(&size);
            if (size > 0)
            {
                IOpndCommand sp = new COpndCommand(commands, isNegative, isDecimal, isExpMode);
                m_commands.Append(sp);
            }
            Recalculate();
        }

        void OnMaxDigitsReached()
        {
            String announcement = LocalizationStringUtil.GetLocalizedNarratorAnnouncement(
                CalculatorResourceKeys.MaxDigitsReachedFormat, m_localizedMaxDigitsReachedAutomationFormat, m_CalculationResultAutomationName.Data());

            Announcement = CalculatorAnnouncement.GetMaxDigitsReachedAnnouncement(announcement);
        }

        void OnBinaryOperatorReceived()
        {
            Announcement = GetDisplayUpdatedNarratorAnnouncement();
        }

        NarratorAnnouncement GetDisplayUpdatedNarratorAnnouncement()
        {
            String announcement;
            if (m_feedbackForButtonPress == null || m_feedbackForButtonPress.IsEmpty())
            {
                announcement = m_CalculationResultAutomationName;
            }
            else
            {
                announcement = LocalizationStringUtil.GetLocalizedNarratorAnnouncement(
                    CalculatorResourceKeys.ButtonPressFeedbackFormat,
                    m_localizedButtonPressFeedbackAutomationFormat,
                    m_CalculationResultAutomationName.Data(),
                    m_feedbackForButtonPress.Data());
            }

            // Make sure we don't accidentally repeat an announcement.
            m_feedbackForButtonPress = null;

            return CalculatorAnnouncement.GetDisplayUpdatedAnnouncement(announcement);
        }
    }