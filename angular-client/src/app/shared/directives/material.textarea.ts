import { Directive, ElementRef, Renderer2, OnInit, Input } from '@angular/core';

@Directive({
  selector: '[material-textarea]',
})
export class MaterialTextarea implements OnInit {

  @Input() label = "label";
  

  constructor(private elementRef: ElementRef, private renderer: Renderer2) {
  }

  ngOnInit() {
    
    let parentEl: any = this.elementRef.nativeElement.parentElement;
    let div: any = document.createElement("div");
    let span: any = document.createElement("span");
    let labelEl: any = document.createElement("label");
    labelEl.innerText = this.label;

    labelEl.classList.add('label');
    span.classList.add('bar');
    div.classList.add('material-textarea');

    this.elementRef.nativeElement.required = true
    parentEl.appendChild(div);
    div.appendChild(this.elementRef.nativeElement);
    div.appendChild(span);
    div.appendChild(labelEl);
    
  }


}
