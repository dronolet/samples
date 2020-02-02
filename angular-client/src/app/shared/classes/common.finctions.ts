export class CommonFunctions {

  public static isNull(data: any, def: any): any {
    return (data ? data : def);
  }

  public static getArg(data: any, num: number, defValue: string): string {
    let result: string = defValue;
    try {

      if (data) {
        if (data.length > 0 && num < data.length) {
          result = data[num];
        }
      }

    } catch {

    };

    if (!result || result.length == 0)
      result = defValue;

    return result;
  }

  public static getFileContentDisposition(content: string, defaultFile: string): string {
    try {
      return decodeURI(content.split("filename*=UTF-8''")[1].trim());
    } catch {
      return defaultFile;
    }
  }

  public static isnull(value: any, vdef: any): any {
    return (value == null ? vdef : value);
  }

  public static copyObjProps(sObject: any, dObject: any): any {
    let prop: any;
    for (prop in sObject) {
      dObject[prop] = sObject[prop];
    }
    return dObject;
  }

  public static getFileName(fileName: string): string {
    try {
      var splitParts = fileName.split('.');

      return fileName.substring(0, fileName.lastIndexOf('.')).trim();
    } catch {
      return '';
    }
  }

  public static getFileExt(fileName: string, withPoint: boolean = true): string {
    try {
      var splitParts = fileName.split('.');
      if (splitParts.length > 1)
        return (withPoint ? '.' : '') + splitParts[splitParts.length - 1].trim();
      else return '';
    } catch {
      return '';
    }
  }

  public static FileCombine(fileName: string, fileExt: string): string {
    return ((fileName ? fileName : '').trim() + (fileExt ? fileExt : '').trim());
  }

  public static dowloadBlobData(data: any, defaultFile: string) {
    try {

      var blob = new Blob([data.body], { type: "octet/stream" });
      var fileName = this.getFileContentDisposition(data.headers.get("content-disposition"), defaultFile);
      if (window.navigator && window.navigator.msSaveOrOpenBlob) {
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


