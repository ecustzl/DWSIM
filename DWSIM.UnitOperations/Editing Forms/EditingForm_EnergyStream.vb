﻿Imports System.Windows.Forms
Imports DWSIM.Interfaces.Enums.GraphicObjects
Imports DWSIM.SharedClasses.UnitOperations
Imports su = DWSIM.SharedClasses.SystemsOfUnits

Public Class EditingForm_EnergyStream

    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    Public Property SimObject As Streams.EnergyStream

    Public Loaded As Boolean = False

    Dim units As SharedClasses.SystemsOfUnits.Units
    Dim nf As String

    Private Sub EditingForm_HeaterCooler_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        units = SimObject.FlowSheet.FlowsheetOptions.SelectedUnitSystem
        nf = SimObject.FlowSheet.FlowsheetOptions.NumberFormat

        UpdateInfo()

    End Sub

    Sub UpdateInfo()

        Loaded = False

        With SimObject

            'first block

            chkActive.Checked = .GraphicObject.Active

            Me.Text = .GetDisplayName() & ": " & .GraphicObject.Tag

            lblTag.Text = .GraphicObject.Tag
            If .Calculated Then
                lblStatus.Text = .FlowSheet.GetTranslatedString("Calculado") & " (" & .LastUpdated.ToString & ")"
                lblStatus.ForeColor = Drawing.Color.Blue
            Else
                If Not .GraphicObject.Active Then
                    lblStatus.Text = .FlowSheet.GetTranslatedString("Inativo")
                    lblStatus.ForeColor = Drawing.Color.Gray
                ElseIf .ErrorMessage <> "" Then
                    lblStatus.Text = .FlowSheet.GetTranslatedString("Erro") & " (" & .ErrorMessage.Substring(50) & "...)"
                    lblStatus.ForeColor = Drawing.Color.Red
                Else
                    lblStatus.Text = .FlowSheet.GetTranslatedString("NoCalculado")
                    lblStatus.ForeColor = Drawing.Color.Black
                End If
            End If

            lblConnectedTo.Text = ""

            If .IsSpecAttached Then lblConnectedTo.Text = .FlowSheet.SimulationObjects(.AttachedSpecId).GraphicObject.Tag
            If .IsAdjustAttached Then lblConnectedTo.Text = .FlowSheet.SimulationObjects(.AttachedAdjustId).GraphicObject.Tag

            'connections

            Dim objlist As String() = .FlowSheet.SimulationObjects.Values.Where(Function(x) TypeOf x Is DWSIM.SharedClasses.UnitOperations.UnitOpBaseClass Or x.GraphicObject.ObjectType = ObjectType.OT_Recycle).Select(Function(m) m.GraphicObject.Tag).ToArray

            cbInlet1.Items.Clear()
            cbInlet1.Items.AddRange(objlist)

            cbOutlet1.Items.Clear()
            cbOutlet1.Items.AddRange(objlist)

            If .GraphicObject.InputConnectors(0).IsAttached Then
                cbInlet1.SelectedItem = .GraphicObject.InputConnectors(0).AttachedConnector.AttachedFrom.Tag
                tbEnergyFlow.Enabled = False
                cbEnergyFlow.Enabled = False
            End If
            If .GraphicObject.OutputConnectors(0).IsAttached Then cbOutlet1.SelectedItem = .GraphicObject.OutputConnectors(0).AttachedConnector.AttachedTo.Tag

            'annotation

            Try
                rtbAnnotations.Rtf = .Annotation
            Catch ex As Exception

            End Try

            'parameters

            cbEnergyFlow.Items.Clear()
            cbEnergyFlow.Items.AddRange(units.GetUnitSet(Interfaces.Enums.UnitOfMeasure.heatflow).ToArray)
            cbEnergyFlow.SelectedItem = units.heatflow

            tbEnergyFlow.Text = su.Converter.ConvertFromSI(units.volume, SimObject.EnergyFlow.GetValueOrDefault).ToString(nf)

        End With

        Loaded = True

    End Sub

    Private Sub lblTag_TextChanged(sender As Object, e As EventArgs) Handles lblTag.TextChanged
        If Loaded Then SimObject.GraphicObject.Tag = lblTag.Text
        Me.Text = SimObject.GetDisplayName() & ": " & SimObject.GraphicObject.Tag
    End Sub

    Private Sub btnDisconnectI_Click(sender As Object, e As EventArgs) Handles btnDisconnect1.Click

        If cbInlet1.SelectedItem IsNot Nothing Then
            SimObject.FlowSheet.DisconnectObjects(SimObject.GraphicObject.InputConnectors(0).AttachedConnector.AttachedFrom, SimObject.GraphicObject)
            cbInlet1.SelectedItem = Nothing
        End If

    End Sub

    Private Sub btnDisconnectO_Click(sender As Object, e As EventArgs) Handles btnDisconnectOutlet1.Click

        If cbOutlet1.SelectedItem IsNot Nothing Then
            SimObject.FlowSheet.DisconnectObjects(SimObject.GraphicObject, SimObject.GraphicObject.OutputConnectors(0).AttachedConnector.AttachedTo)
            cbOutlet1.SelectedItem = Nothing
        End If

    End Sub

    Private Sub cb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbEnergyFlow.SelectedIndexChanged

        If Loaded Then
            Try
                If sender Is cbEnergyFlow Then
                    tbEnergyFlow.Text = su.Converter.Convert(cbEnergyFlow.SelectedItem.ToString, units.heatflow, Double.Parse(tbEnergyFlow.Text)).ToString(nf)
                    cbEnergyFlow.SelectedItem = units.heatflow
                    UpdateProps(tbEnergyFlow)
                End If
            Catch ex As Exception
                SimObject.FlowSheet.ShowMessage(ex.Message.ToString, Interfaces.IFlowsheet.MessageType.GeneralError)
            End Try
        End If

    End Sub

    Sub UpdateProps(sender As Object)

        If sender Is tbEnergyFlow Then SimObject.EnergyFlow = su.Converter.ConvertToSI(cbEnergyFlow.SelectedItem.ToString, tbEnergyFlow.Text)

        RequestCalc()

    End Sub

    Sub RequestCalc()

        SimObject.FlowSheet.RequestCalculation(SimObject)

    End Sub

    Private Sub tb_TextChanged(sender As Object, e As EventArgs) Handles tbEnergyFlow.TextChanged

        Dim tbox = DirectCast(sender, TextBox)

        If Double.TryParse(tbox.Text, New Double()) Then
            tbox.ForeColor = Drawing.Color.Blue
        Else
            tbox.ForeColor = Drawing.Color.Red
        End If

    End Sub

    Private Sub TextBoxKeyDown(sender As Object, e As KeyEventArgs) Handles tbEnergyFlow.KeyDown

        If e.KeyCode = Keys.Enter And Loaded And DirectCast(sender, TextBox).ForeColor = Drawing.Color.Blue Then

            UpdateProps(sender)

            DirectCast(sender, TextBox).SelectAll()

        End If

    End Sub

    Private Sub cbInlet_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbInlet1.SelectedIndexChanged
        If Loaded Then UpdateInletConnection(sender)
    End Sub

    Private Sub cbOutlet_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbOutlet1.SelectedIndexChanged
        If Loaded Then UpdateOutletConnection(sender)
    End Sub

    Sub UpdateInletConnection(cb As ComboBox)

        Dim text As String = cb.Text

        If text <> "" Then

            Dim gobj = SimObject.GraphicObject
            Dim flowsheet = SimObject.FlowSheet

            Dim i As Integer = 0
            For Each oc As Interfaces.IConnectionPoint In flowsheet.GetFlowsheetSimulationObject(text).GraphicObject.OutputConnectors
                If Not oc.IsAttached Then
                    If gobj.InputConnectors(0).IsAttached Then flowsheet.DisconnectObjects(gobj.InputConnectors(0).AttachedConnector.AttachedFrom, gobj)
                    flowsheet.ConnectObjects(flowsheet.GetFlowsheetSimulationObject(text).GraphicObject, gobj, i, 0)
                    Exit Sub
                End If
                i += 1
            Next

            MessageBox.Show(flowsheet.GetTranslatedString("Todasasconexespossve"), flowsheet.GetTranslatedString("Erro"), MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

    End Sub

    Sub UpdateOutletConnection(cb As ComboBox)

        Dim text As String = cb.Text

        If text <> "" Then

            Dim gobj = SimObject.GraphicObject
            Dim flowsheet = SimObject.FlowSheet

            Dim i As Integer = 0
            For Each ic As Interfaces.IConnectionPoint In flowsheet.GetFlowsheetSimulationObject(text).GraphicObject.InputConnectors
                If Not ic.IsAttached Then
                    If gobj.OutputConnectors(0).IsAttached Then flowsheet.DisconnectObjects(gobj, gobj.OutputConnectors(0).AttachedConnector.AttachedTo)
                    flowsheet.ConnectObjects(gobj, flowsheet.GetFlowsheetSimulationObject(text).GraphicObject, 0, i)
                    Exit Sub
                End If
                i += 1
            Next

            MessageBox.Show(flowsheet.GetTranslatedString("Todasasconexespossve"), flowsheet.GetTranslatedString("Erro"), MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

    End Sub

    Private Sub rtbAnnotations_RtfChanged(sender As Object, e As EventArgs) Handles rtbAnnotations.RtfChanged
        If Loaded Then SimObject.Annotation = rtbAnnotations.Rtf
    End Sub

    Private Sub chkActive_CheckedChanged(sender As Object, e As EventArgs) Handles chkActive.CheckedChanged
        If Loaded Then SimObject.GraphicObject.Active = chkActive.Checked
    End Sub

End Class