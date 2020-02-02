
export class Order {
  dBegin: Date;
  dEnd: Date;
  buildObject: number;
  location: string;
  workType: string;
  contractor: string;
  employee?: string;
  result: number;
  head?: number;
  remark: string;
  repare: Date;
  repareFakt?: Date;
  userId: number;
  canEdit: number;

  static NewOrder() {
    let order: Order = new Order();
    let currDate = new Date();
    order.userId = null;
    order.dBegin = currDate;
    order.dEnd = currDate;
    order.buildObject = 1;
    order.location = null;
    order.workType = null;
    order.contractor = null;
    order.employee = null;
    order.result = 1;
    order.head = null;
    order.remark = null;
    order.repare = currDate;
    order.repareFakt = null;
    order.canEdit = 1;
    return order;
  }

  static convertUTCDateToLocalDate(date:any):Date {
    var newDate = new Date(date);
    //alert(newDate);
    //newDate.setMinutes(date.getMinutes() - date.getTimezoneOffset());
    return newDate;
  }

 

  

  static parseOrder(order: any): Order {
    let porder: any = { ... order };
    porder.dBegin = new Date(porder.dBegin);
    porder.dEnd = new Date(porder.dEnd);
    porder.repare = new Date(porder.repare);
    if (porder.repareFakt)
      porder.repareFakt = new Date(porder.repareFakt);
    else porder.repareFakt = null;
    return (porder as Order);
  }

 
  constructor() { }

}

