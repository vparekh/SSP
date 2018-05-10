<%@ Page Title="" Language="C#" MasterPageFile="~/Views/SSPEditor.Master" AutoEventWireup="true" CodeBehind="ExplanatoryNotes.aspx.cs" Inherits="SSPWebUI.Views.ExplanatoryNotes" %>
<%@ MasterType VirtualPath="~/Views/SSPEditor.Master" %>
<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Menu" runat="server">
    <span id="tabid" style="display: none">explanatorynotes</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Body" runat="server">
    <asp:HiddenField runat="server" ID="allowtreeedit" />
    
    <div id="dlgInsertReference" style="visibility:hidden" title="References"></div>
     <asp:UpdatePanel ID="notesPanel" runat="server">
         
        <ContentTemplate>
            <asp:TextBox ID="txtProtocolVersion" ClientIDMode="Static" ForeColor="White" ReadOnly="true" BackColor="LightGray" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtProtocolVersionValue" ClientIDMode="Static" ForeColor="White" readonly="true" BackColor="LightGray"  runat="server"></asp:TextBox>
                <div style="margin:10px">
                    <table >
                        <tr>
                            <td style="padding:10px">
                                <asp:Label ID="Label9" runat="server" Text="Note"></asp:Label>
                                <asp:DropDownList ID="lstTags" runat="server" ClientIDMode="AutoID" OnSelectedIndexChanged="lstTags_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="padding:10px">
                                <asp:Label  ID="Label8" runat="server" Text="Note Title"></asp:Label>  <asp:TextBox ID="txtNoteTitle" runat="server"></asp:TextBox>   
                                <asp:Button ID="btnNewNote" OnClick="btnNewNote_Click" runat="server" Text="New" />  
                            </td>
                        </tr>
                    </table>
                </div>                                        
                                                 
                <dx:ASPxRichEdit ID="reNotes" runat="server" 
                    ActiveTabIndex="1" 
                    ShowConfirmOnLosingChanges="False" 
                    OnPreRender="DemoRichEdit_PreRender" 
                    WorkDirectory="~\App_Data\WorkDirectory"                                         
                    ClientInstanceName="myRich" 
                    OnCallback="reNotes_Callback" 
                    AutoSaveMode="On">
                    <RibbonTabs>
                        <dx:RERFileTab>
                        <Groups>
                            <dx:RERFileCommonGroup>
                                <Items>
                                    <dx:RibbonTemplateItem Size="Large">
                                            <Template>
                                            
                                                <%-- custom save to database --%>
                                                    <dx:ASPxButton runat="server" Image-Url="~\Images\save.png" ImagePosition="Top" ID="BtnSave" BackColor="White" Text="Save" Width="20" Height="20" AutoPostBack="false">
                                                            <ClientSideEvents Click="OnNoteSaveClick" />
                                                            <Border BorderStyle="None" />
                                                            <PressedStyle BackColor="White"></PressedStyle>
                                                    </dx:ASPxButton>
                                            
                                            </Template>
                                        </dx:RibbonTemplateItem>
                                    <dx:RERPrintCommand Size="Large">
                                    </dx:RERPrintCommand>
                                </Items>
                            </dx:RERFileCommonGroup>
                        </Groups>
                    </dx:RERFileTab>
                        <dx:RERHomeTab>
                        <Groups>
                            <dx:RERUndoGroup>
                                <Items>
                                    <dx:RERUndoCommand>
                                    </dx:RERUndoCommand>
                                    <dx:RERRedoCommand>
                                    </dx:RERRedoCommand>
                                </Items>
                            </dx:RERUndoGroup>
                            <dx:RERClipboardGroup>
                                <Items>
                                    <dx:RERPasteCommand Size="Large">
                                    </dx:RERPasteCommand>
                                    <dx:RERCutCommand>
                                    </dx:RERCutCommand>
                                    <dx:RERCopyCommand>
                                    </dx:RERCopyCommand>
                                </Items>
                            </dx:RERClipboardGroup>
                            <dx:RERFontGroup>
                                <Items>
                                    <dx:RERFontNameCommand>
                                        <PropertiesComboBox DropDownStyle="DropDown" ValueType="System.Object" Width="150px">
                                        </PropertiesComboBox>
                                    </dx:RERFontNameCommand>
                                    <dx:RERFontSizeCommand>
                                        <PropertiesComboBox DropDownStyle="DropDown" ValueType="System.Int32" Width="60px">
                                        </PropertiesComboBox>
                                    </dx:RERFontSizeCommand>
                                    <dx:RERIncreaseFontSizeCommand>
                                    </dx:RERIncreaseFontSizeCommand>
                                    <dx:RERDecreaseFontSizeCommand>
                                    </dx:RERDecreaseFontSizeCommand>
                                    <dx:RERChangeCaseCommand DropDownMode="False">
                                    </dx:RERChangeCaseCommand>
                                    <dx:RERFontBoldCommand>
                                    </dx:RERFontBoldCommand>
                                    <dx:RERFontItalicCommand>
                                    </dx:RERFontItalicCommand>
                                    <dx:RERFontUnderlineCommand>
                                    </dx:RERFontUnderlineCommand>
                                    <dx:RERFontStrikeoutCommand>
                                    </dx:RERFontStrikeoutCommand>
                                    <dx:RERFontSuperscriptCommand>
                                    </dx:RERFontSuperscriptCommand>
                                    <dx:RERFontSubscriptCommand>
                                    </dx:RERFontSubscriptCommand>
                                    <dx:RERFontColorCommand AutomaticColorItemCaption="Automatic" AutomaticColorItemValue="0" Color="Black" EnableAutomaticColorItem="True" EnableCustomColors="True">
                                    </dx:RERFontColorCommand>
                                    <dx:RERFontBackColorCommand AutomaticColor="" AutomaticColorItemCaption="No Color" AutomaticColorItemValue="16777215" EnableAutomaticColorItem="True" EnableCustomColors="True">
                                    </dx:RERFontBackColorCommand>
                                    <dx:RERClearFormattingCommand>
                                    </dx:RERClearFormattingCommand>
                                </Items>
                            </dx:RERFontGroup>
                            <dx:RERParagraphGroup>
                                <Items>
                                    <dx:RERBulletedListCommand>
                                    </dx:RERBulletedListCommand>
                                    <dx:RERNumberingListCommand>
                                    </dx:RERNumberingListCommand>
                                    <dx:RERMultilevelListCommand>
                                    </dx:RERMultilevelListCommand>
                                    <dx:RERDecreaseIndentCommand>
                                    </dx:RERDecreaseIndentCommand>
                                    <dx:RERIncreaseIndentCommand>
                                    </dx:RERIncreaseIndentCommand>
                                    <dx:RERShowWhitespaceCommand>
                                    </dx:RERShowWhitespaceCommand>
                                    <dx:RERAlignLeftCommand>
                                    </dx:RERAlignLeftCommand>
                                    <dx:RERAlignCenterCommand>
                                    </dx:RERAlignCenterCommand>
                                    <dx:RERAlignRightCommand>
                                    </dx:RERAlignRightCommand>
                                    <dx:RERAlignJustifyCommand>
                                    </dx:RERAlignJustifyCommand>
                                    <dx:RERParagraphLineSpacingCommand DropDownMode="False">
                                    </dx:RERParagraphLineSpacingCommand>
                                    <dx:RERParagraphBackColorCommand AutomaticColor="" AutomaticColorItemCaption="No Color" AutomaticColorItemValue="16777215" EnableAutomaticColorItem="True" EnableCustomColors="True">
                                    </dx:RERParagraphBackColorCommand>
                                </Items>
                            </dx:RERParagraphGroup>
                            <dx:RERStylesGroup>
                                <Items>
                                    <dx:RERChangeStyleCommand MaxColumnCount="10" MaxTextWidth="65px" MinColumnCount="2">
                                        <PropertiesDropDownGallery RowCount="3" />
                                    </dx:RERChangeStyleCommand>
                                </Items>
                            </dx:RERStylesGroup>
                            <dx:REREditingGroup>
                                <Items>
                                    <dx:RERSelectAllCommand>
                                    </dx:RERSelectAllCommand>
                                </Items>
                            </dx:REREditingGroup>
                        </Groups>
                    </dx:RERHomeTab>
                        <dx:RERInsertTab>
                        <Groups>
                            <dx:RERPagesGroup>

                                <Items>
                                    <dx:RERInsertPageBreakCommand Size="Large">
                                    </dx:RERInsertPageBreakCommand>
                                </Items>
                            </dx:RERPagesGroup>
                            <dx:RERTablesGroup>
                                <Items>
                                    <dx:RERInsertTableCommand Size="Large">
                                    </dx:RERInsertTableCommand>
                                </Items>
                            </dx:RERTablesGroup>
                            <dx:RERIllustrationsGroup>
                                <Items>
                                    <dx:RERInsertPictureCommand Size="Large">

                                    </dx:RERInsertPictureCommand>
                                </Items>
                            </dx:RERIllustrationsGroup>
                            <dx:RERLinksGroup>
                                <Items>
                                    <dx:RibbonButtonItem Text="Insert Reference" Name="insertRef" Size="Large">
                                    </dx:RibbonButtonItem>
                                    <%--<dx:RERShowBookmarksFormCommand Size="Large">
                                    </dx:RERShowBookmarksFormCommand>--%>
                                    <dx:RERShowHyperlinkFormCommand Size="Large">
                                    </dx:RERShowHyperlinkFormCommand>
                                </Items>
                            </dx:RERLinksGroup>
                            <dx:RERHeaderAndFooterGroup Text="Header &amp; Footer">
                                <Items>
                                    <dx:REREditPageHeaderCommand Size="Large">
                                    </dx:REREditPageHeaderCommand>
                                    <dx:REREditPageFooterCommand Size="Large">
                                    </dx:REREditPageFooterCommand>
                                    <dx:RERInsertPageNumberFieldCommand Size="Large">
                                    </dx:RERInsertPageNumberFieldCommand>
                                    <dx:RERInsertPageCountFieldCommand Size="Large">
                                    </dx:RERInsertPageCountFieldCommand>
                                </Items>
                            </dx:RERHeaderAndFooterGroup>
                            <dx:RERSymbolsGroup>
                                <Items>
                                    <dx:RERShowSymbolFormCommand Size="Large">
                                    </dx:RERShowSymbolFormCommand>
                                </Items>
                            </dx:RERSymbolsGroup>
                        </Groups>
                    </dx:RERInsertTab>
                        <dx:RERPageLayoutTab>
                        <Groups>
                            <dx:RERPageSetupGroup>
                                <Items>
                                    <dx:RERPageMarginsCommand DropDownMode="False" Size="Large">
                                    </dx:RERPageMarginsCommand>
                                    <dx:RERChangeSectionPageOrientationCommand DropDownMode="False" Size="Large">
                                    </dx:RERChangeSectionPageOrientationCommand>
                                    <dx:RERChangeSectionPaperKindCommand DropDownMode="False" Size="Large">
                                    </dx:RERChangeSectionPaperKindCommand>
                                    <dx:RERSetSectionColumnsCommand DropDownMode="False" Size="Large">
                                    </dx:RERSetSectionColumnsCommand>
                                    <dx:RERInsertBreakCommand DropDownMode="False" Size="Large">
                                    </dx:RERInsertBreakCommand>
                                </Items>
                            </dx:RERPageSetupGroup>
                            <dx:RERBackgroundGroup>
                                <Items>
                                    <dx:RERChangePageColorCommand AutomaticColor="Transparent" AutomaticColorItemCaption="No Color" AutomaticColorItemValue="16777215" Color="Transparent" DropDownMode="False" EnableAutomaticColorItem="True" EnableCustomColors="True" Size="Large">
                                    </dx:RERChangePageColorCommand>
                                </Items>
                            </dx:RERBackgroundGroup>
                        </Groups>
                    </dx:RERPageLayoutTab>
                        <dx:RERViewTab>
                        <Groups>
                            <dx:RERShowGroup>
                                <Items>
                                    <dx:RERToggleShowHorizontalRulerCommand Size="Large">
                                    </dx:RERToggleShowHorizontalRulerCommand>
                                </Items>
                            </dx:RERShowGroup>
                            <dx:RERViewGroup>
                                <Items>
                                    <dx:RERToggleFullScreenCommand Size="Large">
                                    </dx:RERToggleFullScreenCommand>
                                </Items>
                            </dx:RERViewGroup>
                        </Groups>
                    </dx:RERViewTab>
                    </RibbonTabs>
                    <ClientSideEvents  HyperlinkClick="function(s,e) {
                                                                       
               	                              
                        e.handled=true;
                        $('#ddReference').val(e.targetUri);
                                               
                        $('#reReferences').focus();
                                                
                        window.location='References.aspx?target=' + e.targetUri;
                                               
                    }" />

                    <ClientSideEvents CustomCommandExecuted="function(s,e) {
                                                
                        $('#dlgInsertReference').empty();
                        $('#dlgInsertReference').append(references);
                        $('#dlgInsertReference').css('visibility', 'visible'); 
                        $('#dlgInsertReference').dialog('open');                                         
                                                
                                                 
                    }" />
                    <ClientSideEvents BeginCallback="function(s,e) {
                                                
                        }" />
                    <ClientSideEvents EndCallback="function(s,e) {
                                               
                        }" />
    </dx:ASPxRichEdit>
        </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>
