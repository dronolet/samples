import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialInput } from "./directives/material.input";
import { MaterialTextarea } from "./directives/material.textarea";


import {

  DxTooltipModule,
  DxLoadIndicatorModule,
  DxPopupModule,
  DxDataGridModule,
  DxTabPanelModule, DxTemplateModule,
  DxButtonModule, DxTextBoxModule, DxTextAreaModule, DxCheckBoxModule, DxSelectBoxModule, DxDateBoxModule,
  DxFormModule, DxValidatorModule, DxValidationGroupModule, DxValidationSummaryModule,
  DxFilterBuilderModule, DxNumberBoxModule

  //DxTooltipModule, DxLoadIndicatorModule,
  //DxPopupModule, DxDataGridModule, DxTabPanelModule, DxTemplateModule,
  //DxButtonModule, DxTextBoxModule, DxTextAreaModule, DxCheckBoxModule, DxSelectBoxModule, DxDateBoxModule,
  //DxFormModule, DxValidatorModule, DxValidationGroupModule, DxValidationSummaryModule,
  //DxFilterBuilderModule, DxDropDownBoxModule,
  //DxSliderModule, DxRadioGroupModule, DxNumberBoxModule
} from "devextreme-angular";



@NgModule({
  declarations: [MaterialInput, MaterialTextarea],
  imports: [    
    DxTooltipModule,
    DxLoadIndicatorModule,
    DxPopupModule,
    DxDataGridModule,
    DxTabPanelModule, DxTemplateModule,
    DxButtonModule, DxTextBoxModule, DxTextAreaModule, DxCheckBoxModule, DxSelectBoxModule, DxDateBoxModule,
    DxFormModule, DxValidatorModule, DxValidationGroupModule, DxValidationSummaryModule,
    DxFilterBuilderModule, DxNumberBoxModule
  ],
  exports: [
    CommonModule,
    MaterialInput,
    MaterialTextarea,

    DxTooltipModule,
    DxLoadIndicatorModule,
    DxPopupModule,
    DxDataGridModule,
    DxTabPanelModule, DxTemplateModule,
    DxButtonModule, DxTextBoxModule, DxTextAreaModule, DxCheckBoxModule, DxSelectBoxModule, DxDateBoxModule,
    DxFormModule, DxValidatorModule, DxValidationGroupModule, DxValidationSummaryModule,
    DxFilterBuilderModule, DxNumberBoxModule

  ]
})
export class SharedModule { }
