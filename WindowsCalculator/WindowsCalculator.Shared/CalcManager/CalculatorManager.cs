// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using CalculatorApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationManager
{

    public enum CalculatorPrecision
    {
        StandardModePrecision = 16,
        ScientificModePrecision = 32,
        ProgrammerModePrecision = 64
    };

    // Numbering continues from the Enum Command from Command.h
    // with some gap to ensure there is no overlap of these ids
    // when (unsigned char) is performed on these ids
    // they shouldn't fall in any number range greater than 80. So never
    // make the memory command ids go below 330
    public enum MemoryCommand
    {
        MemorizeNumber = 330,
        MemorizedNumberLoad = 331,
        MemorizedNumberAdd = 332,
        MemorizedNumberSubtract = 333,
        MemorizedNumberClearAll = 334,
        MemorizedNumberClear = 335
    };

    struct HISTORYITEMVECTOR
    {
        public CalculatorList<(string, int)> spTokens;
        public CalculatorList<IExpressionCommand> spCommands;
        public string expression;
        public string result;
    };

    struct HISTORYITEM
    {
        public HISTORYITEMVECTOR historyItemVector;
    };

    public interface ICalcDisplay
    {
        void SetPrimaryDisplay(string pszText, bool isError);
        void SetIsInError(bool isInError);
        void SetExpressionDisplay(
             CalculatorList<(string, int)> tokens,
             CalculatorList<IExpressionCommand> commands);
        void SetParenthesisNumber( int  count);
        void OnNoRightParenAdded();
        void MaxDigitsReached(); // not an error but still need to inform UI layer.
        void BinaryOperatorReceived();
        void OnHistoryItemAdded( int  addedItemIndex);
        void SetMemorizedNumbers(List<string> memorizedNumbers);
        void MemoryItemChanged(int  indexOfMemory);
    }


    class CalculatorManager : ICalcDisplay
    {
        public void SetPrimaryDisplay(string displayString, bool isError) => throw new NotImplementedException();
        public void SetIsInError(bool isError) => throw new NotImplementedException();
        public void SetExpressionDisplay(
             CalculatorList<(string, int)> tokens,
             CalculatorList<IExpressionCommand> commands) => throw new NotImplementedException();
        public void SetMemorizedNumbers(List<string> memorizedNumbers) => throw new NotImplementedException();
        public void OnHistoryItemAdded(int addedItemIndex) => throw new NotImplementedException();
        public void SetParenthesisNumber(int parenthesisCount) => throw new NotImplementedException();
        public void OnNoRightParenAdded() => throw new NotImplementedException();
        public void DisplayPasteError() => throw new NotImplementedException();
        public void MaxDigitsReached() => throw new NotImplementedException();
        public void BinaryOperatorReceived() => throw new NotImplementedException();
        public void MemoryItemChanged(int indexOfMemory) => throw new NotImplementedException();

        public CalculatorManager(ICalcDisplay displayCallback, IResourceProvider* resourceProvider) => throw new NotImplementedException();

        public void Reset(bool clearMemory = true) => throw new NotImplementedException();
        public void SetStandardMode() => throw new NotImplementedException();
        public void SetScientificMode() => throw new NotImplementedException();
        public void SetProgrammerMode() => throw new NotImplementedException();
        public void SendCommand(Command command) => throw new NotImplementedException();
        public List<char> SerializeCommands() => throw new NotImplementedException();
        public void DeSerializeCommands(List<char> serializedData) => throw new NotImplementedException();
        public void SerializeMemory() => throw new NotImplementedException();
        public List<long> GetSerializedMemory() => throw new NotImplementedException();
        public void DeSerializeMemory(List<long> serializedMemory) => throw new NotImplementedException();
        public void SerializePrimaryDisplay() => throw new NotImplementedException();
        public List<long> GetSerializedPrimaryDisplay() => throw new NotImplementedException();
        public void DeSerializePrimaryDisplay(List<long> serializedPrimaryDisplay) => throw new NotImplementedException();
        public Command SerializeSavedDegreeMode() => throw new NotImplementedException();

        public void MemorizeNumber() => throw new NotImplementedException();
        public void MemorizedNumberLoad(int value) => throw new NotImplementedException();
        public void MemorizedNumberAdd(int value) => throw new NotImplementedException();
        public void MemorizedNumberSubtract(int value) => throw new NotImplementedException();
        public void MemorizedNumberClear(int value) => throw new NotImplementedException();
        public void MemorizedNumberClearAll() => throw new NotImplementedException();

        public bool IsEngineRecording() => throw new NotImplementedException();
        public List<char> GetSavedCommands() => throw new NotImplementedException();
        public void SetRadix(RADIX_TYPE iRadixType) => throw new NotImplementedException();
        public void SetMemorizedNumbersString() => throw new NotImplementedException();
        public string GetResultForRadix(int radix, int precision) => throw new NotImplementedException();
        public void SetPrecision(int precision) => throw new NotImplementedException();
        public void UpdateMaxIntDigits() => throw new NotImplementedException();
        public char DecimalSeparator() => throw new NotImplementedException();

        public List<HISTORYITEM> GetHistoryItems() => throw new NotImplementedException();
        public List<HISTORYITEM> GetHistoryItems(CalculationManager.CALCULATOR_MODE mode) => throw new NotImplementedException();
        public HISTORYITEM GetHistoryItem(int uIdx) => throw new NotImplementedException();
        public bool RemoveHistoryItem(int uIdx) => throw new NotImplementedException();
        public void ClearHistory() => throw new NotImplementedException();
        public int MaxHistorySize() => throw new NotImplementedException();
        public CalculationManager.Command GetCurrentDegreeMode() => throw new NotImplementedException();
        public void SetHistory(CALCULATOR_MODE eMode, List<HISTORYITEM> history) => throw new NotImplementedException();
        public void SetInHistoryItemLoadMode(bool isHistoryItemLoadMode) => throw new NotImplementedException();
    }
}
