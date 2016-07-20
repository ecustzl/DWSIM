'    Pipe Profile Editor
'    Copyright 2008 Daniel Wagner O. de Medeiros
'
'    This file is part of DWSIM.
'
'    DWSIM is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    DWSIM is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with DWSIM.  If not, see <http://www.gnu.org/licenses/>.

Imports System.Windows.Forms
Imports DWSIM.UnitOperations.UnitOperations.Auxiliary.Pipe
Imports cv = DWSIM.SharedClasses.SystemsOfUnits.Converter

<System.Serializable()> Public Class PipeHydraulicProfileEditor

    Inherits System.Windows.Forms.UserControl

#Region "    Variable declarations"

    Public Event StatusChanged(ByVal e As EventArgs, ByVal statuscode As PipeEditorStatus)

    Public WithEvents GridMalha As System.Windows.Forms.DataGridView
    Public WithEvents CMenu1 As System.Windows.Forms.ContextMenuStrip
    Private components As System.ComponentModel.IContainer
    Public WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolStripMenuItem9 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolStripMenuItem10 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolStripMenuItem11 As System.Windows.Forms.ToolStripMenuItem

    Dim l As Integer
    Dim linha_atual As String() = New String() {}

    Public Shared ACD(27, 2) As String
    Dim DNom(218, 6) As String

    Dim ThisExe As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly
    Dim ThisExeName As String = ThisExe.GetName.Name
    Public WithEvents CBTemplate As New DataGridViewComboBoxCell()
    Public WithEvents CBMat As New DataGridViewComboBoxCell()

    Protected m_profile As PipeProfile

#End Region

    Public PipeOp As UnitOperations.Pipe

    Private Units As SharedClasses.SystemsOfUnits.Units
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton5 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ColBase As System.Windows.Forms.DataGridViewTextBoxColumn
    Private NumberFormat As String = ""

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click

        Me.GridMalha.Columns.Add("C" & Me.GridMalha.Columns.Count + 1, "Null")

        CBTemplate = New DataGridViewComboBoxCell()
        CBMat = New DataGridViewComboBoxCell()

        With CBTemplate
            .FlatStyle = FlatStyle.Popup
            .DropDownWidth = 180
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples"))
            .Value = PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples")
            l = 0
            While Not l = ACD.GetUpperBound(0) + 1
                .Items.Add(ACD(l, 0))
                l = l + 1
            End While
            .Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With

        With CBMat
            .FlatStyle = FlatStyle.Popup
            .DropDownWidth = 100
            .Value = PipeOp.FlowSheet.GetTranslatedString("AoComum")
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoComum"))
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoCarbono"))
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("FerroBottomido"))
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoInoxidvel"))
            .Items.Add("PVC")
            .Items.Add("PVC+PFRV")
            .Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With

        Me.GridMalha.Rows(1).Cells(GridMalha.Columns.Count - 1) = CBTemplate
        Me.GridMalha.Rows(4).Cells(GridMalha.Columns.Count - 1) = CBMat
        Me.GridMalha.Rows(2).Cells(GridMalha.Columns.Count - 1).Value = "1"
        Me.GridMalha.Rows(3).Cells(GridMalha.Columns.Count - 1).Value = "5"
        With Me.GridMalha.Rows(0).Cells(GridMalha.Columns.Count - 1)
            .Value = GridMalha.Columns(GridMalha.ColumnCount - 1).Index + 1
            .ReadOnly = True
            .Style.BackColor = System.Drawing.Color.LightGray
            .Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With


    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click

        Dim col1 As New DataGridViewColumn
        col1.CellTemplate = GridMalha.Columns(0).CellTemplate
        GridMalha.Columns.Insert(GridMalha.CurrentCell.ColumnIndex + 1, col1)
        Dim col2 As New DataGridViewColumn
        For Each col2 In Me.GridMalha.Columns
            Me.GridMalha.Rows(0).Cells(col2.Index).Value = col2.Index + 1
        Next
        col1.Dispose()
        col2.Dispose()

        CBTemplate = New DataGridViewComboBoxCell()
        CBMat = New DataGridViewComboBoxCell()

        With CBTemplate
            .FlatStyle = FlatStyle.Popup
            .DropDownWidth = 180
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples"))
            .Value = PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples")
            l = 0
            While Not l = ACD.GetUpperBound(0) + 1
                .Items.Add(ACD(l, 0))
                l = l + 1
            End While
            .Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With

        With CBMat
            .FlatStyle = FlatStyle.Popup
            .DropDownWidth = 100
            .Value = PipeOp.FlowSheet.GetTranslatedString("AoComum")
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoComum"))
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoCarbono"))
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("FerroBottomido"))
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoInoxidvel"))
            .Items.Add("PVC")
            .Items.Add("PVC+PFRV")
            .Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With

        GridMalha.Rows(1).Cells(GridMalha.CurrentCell.ColumnIndex + 1) = CBTemplate
        GridMalha.Rows(4).Cells(GridMalha.CurrentCell.ColumnIndex + 1) = CBMat
        GridMalha.Rows(2).Cells(GridMalha.CurrentCell.ColumnIndex + 1).Value = "1"
        GridMalha.Rows(3).Cells(GridMalha.CurrentCell.ColumnIndex + 1).Value = "5"
        With GridMalha.Rows(0).Cells(GridMalha.CurrentCell.ColumnIndex + 1)
            '.Value = GridMalha.Columns(GridMalha.CurrentCell.ColumnIndex + 1).Index + 1
            .ReadOnly = True
            .Style.BackColor = System.Drawing.Color.LightGray
            .Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
        CBTemplate.Dispose()
        CBMat.Dispose()

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click

        If GridMalha.Columns.Count <> 1 Then GridMalha.Columns.RemoveAt(GridMalha.CurrentCell.ColumnIndex)
        Dim col2 As New DataGridViewColumn
        For Each col2 In Me.GridMalha.Columns
            Me.GridMalha.Rows(0).Cells(col2.Index).Value = col2.Index + 1
        Next
        col2.Dispose()

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click

        Dim inf As DialogResult

        inf = MessageBox.Show(PipeOp.FlowSheet.GetTranslatedString("CliqueemOKparalimparamalha"), PipeOp.FlowSheet.GetTranslatedString("Limparmalha"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, False)

        If inf = Windows.Forms.DialogResult.OK Then

            GridMalha.Columns.Clear()
            GridMalha.Columns.Add("CCW", "1")
            GridMalha.AllowUserToResizeRows = True
            GridMalha.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
            GridMalha.Rows.Add()
            GridMalha.Rows.Add()
            GridMalha.Rows.Add()
            GridMalha.Rows.Add()
            GridMalha.Rows.Add()
            GridMalha.Rows.Add()
            GridMalha.Rows.Add()
            GridMalha.Rows.Add()
            GridMalha.Rows.Add()
            GridMalha.Rows(0).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Segmento")
            GridMalha.Rows(1).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Tipo")
            GridMalha.Rows(2).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Quantidade")
            GridMalha.Rows(3).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Incrementos")
            GridMalha.Rows(4).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Material")
            GridMalha.Rows(5).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Comprimentom").Replace("(m)", "(" & Units.distance & ")")
            GridMalha.Rows(6).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Elevaom").Replace("(m)", "(" & Units.distance & ")")
            GridMalha.Rows(7).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Dexternoin").Replace("(in.)", "(" & Units.diameter & ")")
            GridMalha.Rows(8).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Dinternoin").Replace("(in.)", "(" & Units.diameter & ")")

            Dim CBTemplate As New DataGridViewComboBoxCell()
            Dim CBMat As New DataGridViewComboBoxCell()

            Dim l As Integer
            Dim linha_atual As String() = New String() {}

            Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(ThisExe.GetManifestResourceStream(ThisExeName & "." & "fittings.dat"), System.Text.Encoding.UTF8, True)
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(";")
                l = 0
                While Not MyReader.EndOfData
                    linha_atual = MyReader.ReadFields()
                    ACD(l, 0) = linha_atual(0)
                    ACD(l, 1) = linha_atual(1)
                    ACD(l, 2) = linha_atual(2)
                    l = l + 1
                End While
            End Using

            With CBTemplate
                .FlatStyle = FlatStyle.Popup
                .DropDownWidth = 180
                .Items.Add(PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples"))
                .Value = PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples")
                l = 0
                While Not l = ACD.GetUpperBound(0) + 1
                    .Items.Add(ACD(l, 0))
                    l = l + 1
                End While
                .Style.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With

            With CBMat
                .FlatStyle = FlatStyle.Popup
                .DropDownWidth = 100
                .Value = PipeOp.FlowSheet.GetTranslatedString("AoComum")
                .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoComum"))
                .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoCarbono"))
                .Items.Add(PipeOp.FlowSheet.GetTranslatedString("FerroBottomido"))
                .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoInoxidvel"))
                .Items.Add("PVC")
                .Items.Add("PVC+PFRV")
                .Style.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With

            GridMalha.Rows(1).Cells(0) = CBTemplate
            GridMalha.Rows(4).Cells(0) = CBMat
            GridMalha.Rows(2).Cells(0).Value = "1"
            GridMalha.Rows(3).Cells(0).Value = "5"
            With GridMalha.Rows(0).Cells(0)
                .Value = GridMalha.Columns(0).Index + 1
                .ReadOnly = True
                .Style.BackColor = System.Drawing.Color.LightGray
                .Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With


        End If

    End Sub

#Region "    Outros"

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PipeHydraulicProfileEditor))
        Me.GridMalha = New System.Windows.Forms.DataGridView()
        Me.ColBase = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CMenu1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem9 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem10 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem11 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton5 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        CType(Me.GridMalha, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CMenu1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GridMalha
        '
        Me.GridMalha.AllowUserToAddRows = False
        Me.GridMalha.AllowUserToOrderColumns = True
        Me.GridMalha.AllowUserToResizeRows = False
        Me.GridMalha.BackgroundColor = System.Drawing.SystemColors.Control
        Me.GridMalha.ColumnHeadersVisible = False
        Me.GridMalha.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColBase})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.FormatProvider = New System.Globalization.CultureInfo("pt-BR")
        DataGridViewCellStyle2.NullValue = "<vazio>"
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.GridMalha.DefaultCellStyle = DataGridViewCellStyle2
        resources.ApplyResources(Me.GridMalha, "GridMalha")
        Me.GridMalha.GridColor = System.Drawing.SystemColors.Control
        Me.GridMalha.MultiSelect = False
        Me.GridMalha.Name = "GridMalha"
        Me.GridMalha.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.MenuText
        DataGridViewCellStyle3.FormatProvider = New System.Globalization.CultureInfo("pt-BR")
        DataGridViewCellStyle3.NullValue = "0"
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.GridMalha.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.FormatProvider = New System.Globalization.CultureInfo("pt-BR")
        Me.GridMalha.RowsDefaultCellStyle = DataGridViewCellStyle4
        Me.GridMalha.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.GridMalha.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridMalha.RowTemplate.DefaultCellStyle.FormatProvider = New System.Globalization.CultureInfo("pt-BR")
        Me.GridMalha.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GridMalha.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        '
        'ColBase
        '
        DataGridViewCellStyle1.NullValue = "<empty>"
        Me.ColBase.DefaultCellStyle = DataGridViewCellStyle1
        resources.ApplyResources(Me.ColBase, "ColBase")
        Me.ColBase.Name = "ColBase"
        '
        'CMenu1
        '
        Me.CMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2, Me.ToolStripMenuItem3, Me.ToolStripMenuItem4, Me.ToolStripMenuItem5, Me.ToolStripMenuItem6, Me.ToolStripMenuItem7, Me.ToolStripMenuItem8, Me.ToolStripMenuItem9, Me.ToolStripMenuItem10, Me.ToolStripMenuItem11})
        Me.CMenu1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.CMenu1.Name = "ContextMenuStrip1"
        Me.CMenu1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.CMenu1.ShowImageMargin = False
        resources.ApplyResources(Me.CMenu1, "CMenu1")
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        resources.ApplyResources(Me.ToolStripMenuItem2, "ToolStripMenuItem2")
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        resources.ApplyResources(Me.ToolStripMenuItem3, "ToolStripMenuItem3")
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        resources.ApplyResources(Me.ToolStripMenuItem4, "ToolStripMenuItem4")
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        resources.ApplyResources(Me.ToolStripMenuItem5, "ToolStripMenuItem5")
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        resources.ApplyResources(Me.ToolStripMenuItem6, "ToolStripMenuItem6")
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        resources.ApplyResources(Me.ToolStripMenuItem7, "ToolStripMenuItem7")
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        resources.ApplyResources(Me.ToolStripMenuItem8, "ToolStripMenuItem8")
        '
        'ToolStripMenuItem9
        '
        Me.ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        resources.ApplyResources(Me.ToolStripMenuItem9, "ToolStripMenuItem9")
        '
        'ToolStripMenuItem10
        '
        Me.ToolStripMenuItem10.Name = "ToolStripMenuItem10"
        resources.ApplyResources(Me.ToolStripMenuItem10, "ToolStripMenuItem10")
        '
        'ToolStripMenuItem11
        '
        Me.ToolStripMenuItem11.Name = "ToolStripMenuItem11"
        resources.ApplyResources(Me.ToolStripMenuItem11, "ToolStripMenuItem11")
        '
        'ToolStrip1
        '
        resources.ApplyResources(Me.ToolStrip1, "ToolStrip1")
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripButton3, Me.ToolStripButton4, Me.ToolStripSeparator1, Me.ToolStripButton5, Me.ToolStripSeparator2, Me.ToolStripLabel1, Me.ToolStripLabel2})
        Me.ToolStrip1.Name = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.add
        resources.ApplyResources(Me.ToolStripButton1, "ToolStripButton1")
        Me.ToolStripButton1.Name = "ToolStripButton1"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.arrow_up
        resources.ApplyResources(Me.ToolStripButton2, "ToolStripButton2")
        Me.ToolStripButton2.Name = "ToolStripButton2"
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton3.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.delete
        resources.ApplyResources(Me.ToolStripButton3, "ToolStripButton3")
        Me.ToolStripButton3.Name = "ToolStripButton3"
        '
        'ToolStripButton4
        '
        Me.ToolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton4.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.cross
        resources.ApplyResources(Me.ToolStripButton4, "ToolStripButton4")
        Me.ToolStripButton4.Name = "ToolStripButton4"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        resources.ApplyResources(Me.ToolStripSeparator1, "ToolStripSeparator1")
        '
        'ToolStripButton5
        '
        Me.ToolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton5.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.bullet_tick
        resources.ApplyResources(Me.ToolStripButton5, "ToolStripButton5")
        Me.ToolStripButton5.Name = "ToolStripButton5"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        resources.ApplyResources(Me.ToolStripSeparator2, "ToolStripSeparator2")
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        resources.ApplyResources(Me.ToolStripLabel1, "ToolStripLabel1")
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        resources.ApplyResources(Me.ToolStripLabel2, "ToolStripLabel2")
        '
        'PipeHydraulicProfileEditor
        '
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.GridMalha)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "PipeHydraulicProfileEditor"
        CType(Me.GridMalha, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CMenu1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public Sub New()

        Me.InitializeComponent()

    End Sub

    Public Function DN(ByVal i As Integer, ByVal k As Integer) As String

        Return DNom(i, k)

    End Function

    Function Kfit(ByVal name2 As String) As Array

        Dim name As String = name2.Substring(name2.IndexOf("[") + 1, name2.Length - name2.IndexOf("[") - 2)

        Dim tmp(1) As Double

        'Curva Normal 90�;30,00;1;
        If name = 0 Then
            tmp(0) = 30
            tmp(1) = 1
        End If
        'Curva Normal 45�;16,00;1;
        If name = 1 Then
            tmp(0) = 16
            tmp(1) = 1
        End If
        'Curva Normal 180�;50,00;1;
        If name = 2 Then
            tmp(0) = 50
            tmp(1) = 1
        End If
        'V�lvula Angular;55,00;1;
        If name = 3 Then
            tmp(0) = 55
            tmp(1) = 1
        End If
        'V�lvula Borboleta (2" a 14");40,00;1;
        If name = 4 Then
            tmp(0) = 40
            tmp(1) = 1
        End If
        'V�lvula Esfera;3,00;1;
        If name = 5 Then
            tmp(0) = 3
            tmp(1) = 1
        End If
        'V�lvula Gaveta (Aberta);8,00;1;
        If name = 6 Then
            tmp(0) = 8
            tmp(1) = 1
        End If
        'V�lvula Globo;340,00;1;
        If name = 7 Then
            tmp(0) = 340
            tmp(1) = 1
        End If
        'V�lvula Lift-Check;600,00;1;
        If name = 8 Then
            tmp(0) = 600
            tmp(1) = 1
        End If
        'V�lvula P� (Poppet Disc);420,00;1;
        If name = 9 Then
            tmp(0) = 420
            tmp(1) = 1
        End If
        'V�lvula Reten��o de Portinhola;100,00;1;
        If name = 10 Then
            tmp(0) = 100
            tmp(1) = 1
        End If
        'V�lvula Stop-Check (Globo);400,00;1;
        If name = 11 Then
            tmp(0) = 400
            tmp(1) = 1
        End If
        'T� (sa�da bilateral);20,00;1;
        If name = 12 Then
            tmp(0) = 20
            tmp(1) = 1
        End If
        'T� (sa�da de lado);60,00;1;
        If name = 13 Then
            tmp(0) = 60
            tmp(1) = 1
        End If
        'Contra��o R�pida d/D = 1/2;9,60;0;
        If name = 14 Then
            tmp(0) = 9.6
            tmp(1) = 0
        End If
        'Contra��o R�pida d/D = 1/4;96,00;0;
        If name = 15 Then
            tmp(0) = 96
            tmp(1) = 0
        End If
        'Contra��o R�pida d/D = 3/4;1,11;0;
        If name = 16 Then
            tmp(0) = 11
            tmp(1) = 0
        End If
        'Entrada Borda;0,25;0;
        If name = 17 Then
            tmp(0) = 0.25
            tmp(1) = 0
        End If
        'Entrada Normal;0,78;0;
        If name = 18 Then
            tmp(0) = 0.78
            tmp(1) = 0
        End If
        'Expans�o R�pida d/D = 1/2;9,00;0;
        If name = 19 Then
            tmp(0) = 9
            tmp(1) = 0
        End If
        'Expans�o R�pida d/D = 1/4;225,00;0;
        If name = 20 Then
            tmp(0) = 225
            tmp(1) = 0
        End If
        'Expans�o R�pida d/D = 3/4;0,60;0;
        If name = 21 Then
            tmp(0) = 0.6
            tmp(1) = 0
        End If
        'Joelho em 90�;60,00;1;
        If name = 22 Then
            tmp(0) = 60
            tmp(1) = 1
        End If
        'Redu��o Normal 2:1;5,67;0;
        If name = 23 Then
            tmp(0) = 5.67
            tmp(1) = 0
        End If
        'Redu��o Normal 4:3;0,65;0;
        If name = 24 Then
            tmp(0) = 0.65
            tmp(1) = 0
        End If
        'Sa�da Borda;1,00;0;
        If name = 25 Then
            tmp(0) = 1
            tmp(1) = 0
        End If
        'Sa�da Normal;1,00;0;
        If name = 26 Then
            tmp(0) = 1
            tmp(1) = 0
        End If

        Kfit = tmp

    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then

            If (components IsNot Nothing) Then
                components.Dispose()
            End If

        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub GridMalha_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridMalha.CurrentCellChanged

        If GridMalha.Created = True And Not GridMalha.CurrentRow Is Nothing Then

            Dim x As Integer
            x = GridMalha.CurrentRow.Index
            If x = 1 Then
                If GridMalha.CurrentCell.Value <> PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples") Then
                    GridMalha.Rows(3).Cells(GridMalha.CurrentCell.ColumnIndex).ReadOnly = True
                    GridMalha.Rows(5).Cells(GridMalha.CurrentCell.ColumnIndex).ReadOnly = True
                    GridMalha.Rows(6).Cells(GridMalha.CurrentCell.ColumnIndex).ReadOnly = True
                    GridMalha.Rows(7).Cells(GridMalha.CurrentCell.ColumnIndex).ReadOnly = True
                    GridMalha.Rows(3).Cells(GridMalha.CurrentCell.ColumnIndex).Style.BackColor = System.Drawing.Color.LightGray
                    GridMalha.Rows(5).Cells(GridMalha.CurrentCell.ColumnIndex).Style.BackColor = System.Drawing.Color.LightGray
                    GridMalha.Rows(6).Cells(GridMalha.CurrentCell.ColumnIndex).Style.BackColor = System.Drawing.Color.LightGray
                    GridMalha.Rows(7).Cells(GridMalha.CurrentCell.ColumnIndex).Style.BackColor = System.Drawing.Color.LightGray
                Else
                    GridMalha.Rows(3).Cells(GridMalha.CurrentCell.ColumnIndex).ReadOnly = False
                    GridMalha.Rows(5).Cells(GridMalha.CurrentCell.ColumnIndex).ReadOnly = False
                    GridMalha.Rows(6).Cells(GridMalha.CurrentCell.ColumnIndex).ReadOnly = False
                    GridMalha.Rows(7).Cells(GridMalha.CurrentCell.ColumnIndex).ReadOnly = False
                    GridMalha.Rows(3).Cells(GridMalha.CurrentCell.ColumnIndex).Style.BackColor = Nothing
                    GridMalha.Rows(5).Cells(GridMalha.CurrentCell.ColumnIndex).Style.BackColor = Nothing
                    GridMalha.Rows(6).Cells(GridMalha.CurrentCell.ColumnIndex).Style.BackColor = Nothing
                    GridMalha.Rows(7).Cells(GridMalha.CurrentCell.ColumnIndex).Style.BackColor = Nothing
                End If
            End If
        End If

    End Sub

    Private Sub GridMalha_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles GridMalha.DataError



    End Sub



    Private Sub GridMalha_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridMalha.MouseDoubleClick

        If GridMalha.CurrentCell.RowIndex = 7 And GridMalha.CurrentCell.ReadOnly = False Then

            CMenu1.Show(MousePosition.X, MousePosition.Y)
            CMenu1.SetBounds(MousePosition.X, MousePosition.Y, 256, 20)

        End If

    End Sub

    Private Sub ToolStripMenuItem2_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem2.DropDownItemClicked

        Dim idx = ToolStripMenuItem2.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 25 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, DN(r, 1)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, DN(r, 6)), NumberFormat)

    End Sub

    Private Sub ToolStripMenuItem3_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem3.DropDownItemClicked

        Dim idx = ToolStripMenuItem3.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 43 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, DN(r, 1)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, DN(r, 6)), NumberFormat)

    End Sub

    Private Sub ToolStripMenuItem4_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem4.DropDownItemClicked

        Dim idx = ToolStripMenuItem4.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 55 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, DN(r, 1)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, DN(r, 6)), NumberFormat)

    End Sub

    Private Sub ToolStripMenuItem5_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem5.DropDownItemClicked

        Dim idx = ToolStripMenuItem5.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 65 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, DN(r, 1)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, DN(r, 6)), NumberFormat)

    End Sub

    Private Sub ToolStripMenuItem6_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem6.DropDownItemClicked

        Dim idx = ToolStripMenuItem6.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 79 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, DN(r, 1)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, DN(r, 6)), NumberFormat)

    End Sub

    Private Sub ToolStripMenuItem7_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem7.DropDownItemClicked

        Dim idx = ToolStripMenuItem7.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 86 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, Double.Parse(DN(r, 1), System.Globalization.CultureInfo.InvariantCulture)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, Double.Parse(DN(r, 6), System.Globalization.CultureInfo.InvariantCulture)), NumberFormat)

    End Sub

    Private Sub ToolStripMenuItem8_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem8.DropDownItemClicked

        Dim idx = ToolStripMenuItem8.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 98 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, DN(r, 1)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, DN(r, 6)), NumberFormat)

    End Sub

    Private Sub ToolStripMenuItem9_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem9.DropDownItemClicked

        Dim idx = ToolStripMenuItem9.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 109 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, DN(r, 1)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, DN(r, 6)), NumberFormat)

    End Sub

    Private Sub ToolStripMenuItem10_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem10.DropDownItemClicked

        Dim idx = ToolStripMenuItem10.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 122 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, DN(r, 1)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, DN(r, 6)), NumberFormat)

    End Sub

    Private Sub ToolStripMenuItem11_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStripMenuItem11.DropDownItemClicked

        Dim idx = ToolStripMenuItem11.DropDownItems.IndexOf(e.ClickedItem)

        Dim r = idx + 135 - 4
        GridMalha.CurrentCell.Value = Format(cv.Convert("in", Units.diameter, DN(r, 1)), NumberFormat)
        GridMalha.Rows(GridMalha.CurrentRow.Index + 1).Cells(GridMalha.CurrentCell.ColumnIndex).Value = Format(cv.Convert("in", Units.diameter, DN(r, 6)), NumberFormat)

    End Sub

#End Region

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click

        Dim column As New DataGridViewColumn
        Dim v1, v2, v3, v4, v5, v6, v7, v8, v9 As Object

        If Not PipeOp.Profile Is Nothing Then PipeOp.Profile.Sections.Clear()
        For Each column In Me.GridMalha.Columns
            If ParseColumn(column) = "OK" Then
                v1 = column.Index + 1
                v2 = Me.GridMalha.Rows(1).Cells(column.Name).Value
                v3 = Me.GridMalha.Rows(2).Cells(column.Name).Value
                v4 = Me.GridMalha.Rows(3).Cells(column.Name).Value
                v5 = Me.GridMalha.Rows(4).Cells(column.Name).Value
                v6 = Me.GridMalha.Rows(5).Cells(column.Name).Value
                v7 = Me.GridMalha.Rows(6).Cells(column.Name).Value
                v8 = Me.GridMalha.Rows(7).Cells(column.Name).Value
                v9 = Me.GridMalha.Rows(8).Cells(column.Name).Value

                If v2 = PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples") Then v2 = "Tubulaosimples"

                PipeOp.Profile.Sections.Add(column.Index + 1, New PipeSection(v1, v2, v3, v4, v5, cv.Convert(Me.Units.distance, "m", v6), cv.Convert(Me.Units.distance, "m", v7), cv.Convert(Me.Units.diameter, "in", v8), cv.Convert(Me.Units.diameter, "in", v9)))
            Else
                ToolStripLabel2.Text = PipeOp.FlowSheet.GetTranslatedString("Erronasecao") & " " & column.Index + 1 & "."
                RaiseEvent StatusChanged(e, PipeEditorStatus.Erro)
                Exit Sub
            End If
        Next
        RaiseEvent StatusChanged(e, PipeEditorStatus.OK)

        column.Dispose()

    End Sub

    Public Sub handleStatus(ByVal e As EventArgs, ByVal statuscode As PipeEditorStatus) Handles Me.StatusChanged

        If statuscode = PipeEditorStatus.Erro Then
            'Label1.Text = "Erro"
            ToolStripLabel2.ForeColor = Drawing.Color.Red
            PipeOp.Profile.Status = PipeEditorStatus.Definir
        ElseIf statuscode = PipeEditorStatus.Definir Then
            ToolStripLabel2.Text = PipeOp.FlowSheet.GetTranslatedString("Indefinido")
            ToolStripLabel2.ForeColor = Drawing.Color.Red
            PipeOp.Profile.Status = PipeEditorStatus.Definir
        ElseIf statuscode = PipeEditorStatus.OK Then
            ToolStripLabel2.Text = "OK"
            ToolStripLabel2.ForeColor = Drawing.Color.Green
            PipeOp.Profile.Status = PipeEditorStatus.OK
        End If

    End Sub

    Private Function ParseColumn(ByVal column As DataGridViewColumn)
        Try
            With (Me.GridMalha)
                If Not Convert.ToDouble(.Rows(2).Cells(column.Name).Value) > 0.0# Then
                    Return PipeOp.FlowSheet.GetTranslatedString("Erro")
                    Exit Function
                End If
                If .Rows(1).Cells(column.Name).Value = PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples") Then
                    If Not .Rows(3).Cells(column.Name).Value > 0.0# Then
                        Return PipeOp.FlowSheet.GetTranslatedString("Erro")
                        Exit Function
                    End If
                    If Not Convert.ToDouble(.Rows(5).Cells(column.Name).Value) > 0.0# Or Double.IsNaN(.Rows(5).Cells(column.Name).Value) Then
                        Return PipeOp.FlowSheet.GetTranslatedString("Erro")
                        Exit Function
                    End If
                    If Double.IsNaN(.Rows(6).Cells(column.Name).Value) Then
                        Return PipeOp.FlowSheet.GetTranslatedString("Erro")
                        Exit Function
                    End If
                    If Not Convert.ToDouble(.Rows(7).Cells(column.Name).Value) > 0.0# Or Double.IsNaN(.Rows(7).Cells(column.Name).Value) Then
                        Return PipeOp.FlowSheet.GetTranslatedString("Erro")
                        Exit Function
                    End If
                    If Not Convert.ToDouble(.Rows(8).Cells(column.Name).Value) > 0.0# Or Double.IsNaN(.Rows(8).Cells(column.Name).Value) Or CDbl(.Rows(8).Cells(column.Name).Value) > CDbl(.Rows(7).Cells(column.Name).Value) Then
                        Return PipeOp.FlowSheet.GetTranslatedString("Erro")
                        Exit Function
                    End If
                End If
            End With
        Catch ex As Exception
            Return PipeOp.FlowSheet.GetTranslatedString("Erro")
            Exit Function
        End Try
        Return "OK"

    End Function

    Private Sub ConvertProfileToGrid(ByVal Profile As PipeProfile)

        Dim psec As New PipeSection

        'If Not Me.GridMalha.Columns Is Nothing Then Me.GridMalha.Columns.Clear()
        For Each psec In Profile.Sections.Values
            Me.Button8_Click(Nothing, Nothing)
            Me.GridMalha.Rows(0).Cells(psec.Indice - 1).Value = psec.Indice
            If Not CBTemplate.Items.Contains(psec.TipoSegmento) Then
                Me.GridMalha.Rows(1).Cells(psec.Indice - 1).Value = CBTemplate.Items(0)
            Else
                Me.GridMalha.Rows(1).Cells(psec.Indice - 1).Value = psec.TipoSegmento
            End If
            Me.GridMalha.Rows(2).Cells(psec.Indice - 1).Value = psec.Quantidade
            Me.GridMalha.Rows(3).Cells(psec.Indice - 1).Value = psec.Incrementos
            If Not CBMat.Items.Contains(psec.Material) Then
                Me.GridMalha.Rows(4).Cells(psec.Indice - 1).Value = CBMat.Items(0)
            Else
                Me.GridMalha.Rows(4).Cells(psec.Indice - 1).Value = psec.Material
            End If
            Me.GridMalha.Rows(5).Cells(psec.Indice - 1).Value = Format(cv.Convert("m", Me.Units.distance, psec.Comprimento), NumberFormat)
            Me.GridMalha.Rows(6).Cells(psec.Indice - 1).Value = Format(cv.Convert("m", Me.Units.distance, psec.Elevacao), NumberFormat)
            Me.GridMalha.Rows(7).Cells(psec.Indice - 1).Value = Format(cv.Convert("in", Me.Units.diameter, psec.DE), NumberFormat)
            Me.GridMalha.Rows(8).Cells(psec.Indice - 1).Value = Format(cv.Convert("in", Me.Units.diameter, psec.DI), NumberFormat)
        Next
        psec = Nothing

    End Sub

    Private Sub PipeEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim l, j As Integer
        Dim linha_atual As String() = New String() {}

        Using MyReader2 As New Microsoft.VisualBasic.FileIO.TextFieldParser(ThisExe.GetManifestResourceStream(ThisExeName & "." & "pipes.dat"))
            MyReader2.TextFieldType = FileIO.FieldType.Delimited
            MyReader2.SetDelimiters(";")
            l = 0
            While Not MyReader2.EndOfData
                linha_atual = MyReader2.ReadFields()
                j = 0
                Do
                    DNom(l, j) = linha_atual(j)
                    j = j + 1
                Loop Until j = 7
                l = l + 1
            End While
        End Using

        Dim r, aux, linha_inicial, linha_final As Integer

        linha_inicial = 25
        linha_final = 30
        r = linha_inicial - 4
        With ToolStripMenuItem2.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                    & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        linha_inicial = 43
        linha_final = 48
        r = linha_inicial - 4
        With ToolStripMenuItem3.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                 & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        linha_inicial = 55
        linha_final = 60
        r = linha_inicial - 4
        With ToolStripMenuItem4.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                 & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        linha_inicial = 65
        linha_final = 71
        r = linha_inicial - 4
        With ToolStripMenuItem5.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                 & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        linha_inicial = 79
        linha_final = 85
        r = linha_inicial - 4
        With ToolStripMenuItem6.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                 & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        linha_inicial = 86
        linha_final = 97
        r = linha_inicial - 4
        With ToolStripMenuItem7.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                 & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        linha_inicial = 98
        linha_final = 108
        r = linha_inicial - 4
        With ToolStripMenuItem8.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                 & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        linha_inicial = 109
        linha_final = 121
        r = linha_inicial - 4
        With ToolStripMenuItem9.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                 & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        linha_inicial = 122
        linha_final = 134
        r = linha_inicial - 4

        With ToolStripMenuItem10.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                 & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        linha_inicial = 135
        linha_final = 146
        r = linha_inicial - 4
        With ToolStripMenuItem11.DropDownItems
            aux = .Count
            If aux <> 0 Then .Clear()
            Do
                .Add(DN(r, 2) & " / " & DN(r, 3) _
                 & " / " & DN(r, 4))
                r = r + 1
            Loop Until r = linha_final - 3
        End With

        '======================================

        GridMalha.AllowUserToResizeRows = True
        GridMalha.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        GridMalha.Rows.Add()
        GridMalha.Rows.Add()
        GridMalha.Rows.Add()
        GridMalha.Rows.Add()
        GridMalha.Rows.Add()
        GridMalha.Rows.Add()
        GridMalha.Rows.Add()
        GridMalha.Rows.Add()
        GridMalha.Rows.Add()
        GridMalha.Rows(0).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Segmento")
        GridMalha.Rows(1).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Tipo")
        GridMalha.Rows(2).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Quantidade")
        GridMalha.Rows(3).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Incrementos")
        GridMalha.Rows(4).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Material")
        GridMalha.Rows(5).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Comprimentom")
        GridMalha.Rows(6).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Elevaom")
        GridMalha.Rows(7).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Dexternoin")
        GridMalha.Rows(8).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Dinternoin")

        CBTemplate = New DataGridViewComboBoxCell()
        CBMat = New DataGridViewComboBoxCell()

        linha_atual = New String() {}

        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(ThisExe.GetManifestResourceStream(ThisExeName & "." & "fittings.dat"), System.Text.Encoding.Default, True)
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(";")
            l = 0
            While Not MyReader.EndOfData
                linha_atual = MyReader.ReadFields()
                ACD(l, 0) = linha_atual(0)
                ACD(l, 1) = linha_atual(1)
                ACD(l, 2) = linha_atual(2)
                l = l + 1
            End While
        End Using

        With CBTemplate
            .FlatStyle = FlatStyle.Popup
            .DropDownWidth = 180
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples"))
            .Value = PipeOp.FlowSheet.GetTranslatedString("Tubulaosimples")
            l = 0
            While Not l = ACD.GetUpperBound(0) + 1
                .Items.Add(ACD(l, 0))
                l = l + 1
            End While
            .Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With

        With CBMat
            .FlatStyle = FlatStyle.Popup
            .DropDownWidth = 100
            .Value = PipeOp.FlowSheet.GetTranslatedString("AoComum")
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoComum"))
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoCarbono"))
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("FerroBottomido"))
            .Items.Add(PipeOp.FlowSheet.GetTranslatedString("AoInoxidvel"))
            .Items.Add("PVC")
            .Items.Add("PVC+PFRV")
            .Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With

        GridMalha.Rows(1).Cells(0) = CBTemplate
        GridMalha.Rows(4).Cells(0) = CBMat
        GridMalha.Rows(2).Cells(0).Value = "1"
        GridMalha.Rows(3).Cells(0).Value = "5"
        With GridMalha.Rows(0).Cells(0)
            .Value = GridMalha.Columns(0).Index + 1
            .ReadOnly = True
            .Style.BackColor = System.Drawing.Color.LightGray
            .Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With

        Units = PipeOp.FlowSheet.FlowsheetOptions.SelectedUnitSystem
        NumberFormat = PipeOp.FlowSheet.FlowsheetOptions.NumberFormat

        If Not PipeOp.Profile Is Nothing Then
            GridMalha.Rows(5).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Comprimentom").Replace("(m)", "(" & Units.distance & ")")
            GridMalha.Rows(6).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Elevaom").Replace("(m)", "(" & Units.distance & ")")
            GridMalha.Rows(7).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Dexternoin").Replace("(in.)", "(" & Units.diameter & ")")
            GridMalha.Rows(8).HeaderCell.Value = PipeOp.FlowSheet.GetTranslatedString("Dinternoin").Replace("(in.)", "(" & Units.diameter & ")")
            If PipeOp.Profile.Sections.Count > 0 Then
                Me.ConvertProfileToGrid(PipeOp.Profile)
                Me.GridMalha.Columns.RemoveAt(Me.GridMalha.Columns.Count - 1)
                ToolStripLabel2.Text = "OK"
                ToolStripLabel2.ForeColor = Drawing.Color.Green
                PipeOp.Profile.Status = PipeEditorStatus.OK
            End If
        End If

    End Sub

  
End Class
