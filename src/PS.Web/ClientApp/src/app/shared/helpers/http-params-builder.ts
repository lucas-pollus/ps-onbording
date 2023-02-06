import { HttpParams } from '@angular/common/http';

export class HttpParamsBuilder {
  /**
   * append all params
   * @param args {any[]}
   * @returns {HttpParams}
   */
  static appendAll(...args: any[]): HttpParams {
    let params = new HttpParams();
    args.forEach((param) => {
      Object.keys(param).forEach((key) => {
        if (param[key] != null || undefined) {
          params = params.append(key, param[key]);
        }
      });
    });
    return params;
  }
}
