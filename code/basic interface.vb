Sub query()

    Application.ScreenUpdating = False
    
    Dim today As String, frequency As String, formula As String, tickerCell As String, functionCells As String
    Dim variables() As String, tickers() As String
    
    Dim i As Integer, j As Integer, n As Integer, m As Integer, worksheetCount As Integer
    Dim indexes() As Integer
    
    today = CStr(Date)
    
    If IsEmpty(worksheets("query").Range("G2").Value) Then StartDate = CStr("01/01/1970") Else StartDate = CStr(worksheets("query").Range("G2").Value)
    If IsEmpty(worksheets("query").Range("G3").Value) Then EndDate = today Else EndDate = CStr(worksheets("query").Range("G3").Value)
    If IsEmpty(worksheets("query").Range("G4").Value) Then frequency = "d" Else frequency = CStr(worksheets("query").Range("G4").Value)
    
    worksheetCount = ActiveWorkbook.worksheets.Count
    
    Dim sheetnames() As String: ReDim sheetnames(worksheetCount - 2)
    sheetnames = getsheetnames()
    
    
    For i = 0 To (worksheetCount - 2)
    
        worksheets(sheetnames(i)).Activate
        
        n = getnumberofvariables(sheetnames(i))
        ReDim variables(n - 1): variables = getvariables(n, sheetnames(i))
    
        m = getnumberoftickers(sheetnames(i))
        ReDim tickers(m - 1): tickers = gettickers(m, sheetnames(i))
    
        ReDim indexes(m - 1): indexes = gettickerindexes(m, sheetnames(i))
    
        Rows("4:4").Select: Range(Selection, Selection.End(xlDown)).Select
        Selection.ClearContents
            
        For j = 1 To (UBound(indexes) + 1)
            Cells(1, indexes(j - 1)).Value = tickers(j - 1)
            Cells(2, indexes(j - 1)).Value = "DATE"
            Range(Cells(2, indexes(j - 1) + 1), Cells(2, indexes(j - 1) + 1)).Resize(1, UBound(variables) + 1) = variables
            tickerCell = "R1C" + CStr(indexes(j - 1))
            functionCells = "R2C" + CStr(indexes(j - 1) + 1) + ":R2C" + CStr(indexes(j - 1) + UBound(variables) + 1)
            formula = "=BDH(" + tickerCell + "," + functionCells + "," + Chr(34) + StartDate + Chr(34) + "," + Chr(34) + EndDate + Chr(34) + ",""Dir=V"",""Dts=S"",""Sort=A"",""Quote=C"",""QtTyp=Y"",""Days=T" + Chr(34) + "," + Chr(34) + CStr("Per=c" + frequency) + Chr(34) + "," + Chr(34) + "DtFmt=D"",""UseDPDF=Y"",""CshAdjNormal=N"",""CshAdjAbnormal=N"",""CapChg=N"",""cols=2;rows=413"")"
            Cells(3, indexes(j - 1)).FormulaR1C1 = formula
        Next j
    
        Cells.Replace What:="=", Replacement:="=", LookAt:=xlPart, SearchOrder:=xlByRows, MatchCase:=False, SearchFormat:=False, ReplaceFormat:=False
        
    Next i
    
    sheets("query").Select
    Range("A1").Select
    
    End Sub
    
    
    Private Function getsheetnames() As Variant
    
    Dim numberofsheets As Integer, i As Integer
    numberofsheets = ActiveWorkbook.sheets.Count
    Dim sheetnames() As String: ReDim sheetnames(numberofsheets - 2)
    
    For i = 1 To numberofsheets
        If ActiveWorkbook.sheets(i).Name <> "query" Then sheetnames(i - 1) = ActiveWorkbook.sheets(i).Name
    Next i
    
    getsheetnames = sheetnames
    
    End Function
    
    Private Function getnumberofvariables(sheetname As String) As Integer
    
    Dim n As Integer
    
    Range("B2").Select
    
    While Not IsEmpty(ActiveCell)
        ActiveCell.Offset(0, 1).Range("A1").Select
        n = n + 1
    Wend
    
    
    getnumberofvariables = n
        
    End Function
    
    Private Function getvariables(numberofvariables As Integer, sheetname As String) As Variant
    
    Dim n As Integer
    Dim variables() As String: ReDim variables(numberofvariables - 1)
    
    worksheets(sheetname).Activate
    
    Range("B2").Select
    n = 0
    
    While Not IsEmpty(ActiveCell)
        variables(n) = CStr(ActiveCell.Value)
        ActiveCell.Offset(0, 1).Range("A1").Select
        n = n + 1
    Wend
    
    getvariables = variables
        
    End Function
    
    Private Function getnumberoftickers(sheetname As String) As Integer
        
    Dim n As Integer, i As Integer, LastCol As Integer
    
    LastCol = ActiveSheet.Cells(1, ActiveSheet.Columns.Count).End(xlToLeft).Column
    
    Range("A1").Select
    n = 0
    
    For i = 1 To (LastCol)
        If Not IsEmpty(ActiveCell) Then n = n + 1
        ActiveCell.Offset(0, 1).Range("A1").Select
    Next i
        
        
    getnumberoftickers = n
        
    End Function
    
    Private Function gettickers(numberoftickers As Integer, sheetname As String) As Variant
    
    Dim tickers() As String: ReDim tickers(numberoftickers - 1)
    Dim i As Integer, n As Integer, LastCol As Integer
    
    LastCol = ActiveSheet.Cells(1, ActiveSheet.Columns.Count).End(xlToLeft).Column
    Range("A1").Select
    n = 0
    
    For i = 1 To (LastCol)
        If Not IsEmpty(ActiveCell) Then
            tickers(n) = CStr(ActiveCell.Value)
            n = n + 1
        End If
        ActiveCell.Offset(0, 1).Range("A1").Select
    Next i
        
    gettickers = tickers
    
    End Function
    
    Private Function gettickerindexes(numberoftickers As Integer, sheetname As String) As Variant
    
    Dim indexes() As Integer: ReDim indexes(numberoftickers - 1)
    Dim i As Integer, n As Integer, LastCol As Integer
    
    
    LastCol = ActiveSheet.Cells(1, ActiveSheet.Columns.Count).End(xlToLeft).Column
    Range("A1").Select
    n = 0
    
    For i = 1 To (LastCol)
        If Not IsEmpty(ActiveCell) Then
            indexes(n) = i
            n = n + 1
        End If
        ActiveCell.Offset(0, 1).Range("A1").Select
    Next i
        
    gettickerindexes = indexes
    
    End Function
    