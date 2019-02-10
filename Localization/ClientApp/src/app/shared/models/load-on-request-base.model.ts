import { Input } from '@angular/core';

export class LoadOnRequestBase {

  @Input()
  set triggerLoading(triggerred: boolean) {
    if (triggerred) {
      this.load();
    }
  }

  load() {
  }

}
