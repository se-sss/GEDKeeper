object fmNoteEdit: TfmNoteEdit
  Left = 456
  Top = 160
  BorderStyle = bsDialog
  Caption = #1047#1072#1084#1077#1090#1082#1072
  ClientHeight = 249
  ClientWidth = 402
  Color = clBtnFace
  DefaultMonitor = dmMainForm
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  KeyPreview = True
  OldCreateOrder = False
  Position = poScreenCenter
  OnKeyDown = FormKeyDown
  PixelsPerInch = 96
  TextHeight = 13
  object btnAccept: TBitBtn
    Left = 224
    Top = 216
    Width = 81
    Height = 25
    Caption = #1055#1088#1080#1085#1103#1090#1100
    TabOrder = 1
    OnClick = btnAcceptClick
    Kind = bkOK
  end
  object btnCancel: TBitBtn
    Left = 312
    Top = 216
    Width = 81
    Height = 25
    Caption = #1054#1090#1084#1077#1085#1080#1090#1100
    TabOrder = 2
    Kind = bkCancel
  end
  object MemoNote: TMemo
    Left = 8
    Top = 8
    Width = 385
    Height = 193
    ScrollBars = ssBoth
    TabOrder = 0
  end
end
