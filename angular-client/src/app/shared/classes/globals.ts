

export class Globals{

  public static DaysDiff(date1: Date, date2: Date): number {

    var one_day = 1000 * 60 * 60 * 24;
    return Math.round(Math.abs(date2.getTime() - date1.getTime())) / (one_day); 
  }

  static DateWithotTime(date: Date): Date {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate());
  }

  static addDays(date: Date, days: number): Date {
    const copy = new Date(Number(date))
    copy.setDate(date.getDate() + days)
    return copy
  }

  static addMonth(date: Date, months: number): Date {
    const copy = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    copy.setMonth(copy.getMonth() + months);
    return copy;
  }

  static toUTC(date: Date): Date {
    return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
  }


  public static getFileName(fileName: string): string {
    try {
      var splitParts = fileName.split('.');

      return fileName.substring(0, fileName.lastIndexOf('.')).trim();
    } catch {
      return '';
    }
  }

  public static getFileContentDisposition(content: string, defaultFile: string): string {
    try {
      return decodeURI(content.split("filename*=UTF-8''")[1].trim());
    } catch {
      return defaultFile;
    }
  }

  public static dowloadBlobData(data: any, defaultFile: string) {
    try {
      var blob = new Blob([data.body], { type: "octet/stream" });
      var fileName = this.getFileContentDisposition(data.headers.get("content-disposition"), defaultFile);
      if (window.navigator && window.navigator.msSaveOrOpenBlob) { // for IE
        window.navigator.msSaveOrOpenBlob(blob, fileName);
      } else {
        var a = document.createElement("a");
        document.body.appendChild(a);
        a.style.display = "none";
        var url = window.URL.createObjectURL(blob);
        a.href = url;
        a.download = fileName;
        a.click();
        window.URL.revokeObjectURL(url);
      }
    } catch {

    }
  }

} 


