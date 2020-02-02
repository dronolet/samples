
export class Filter {

  from: Date;
  to: Date;
  isOverdue: boolean;

  public static getNew(): Filter {
    return new Filter(); 
  }

  constructor() {
    this.from = new Date();
    this.to = new Date();
    this.isOverdue = false;
  }
}

