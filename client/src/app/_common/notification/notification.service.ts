import {Injectable} from '@angular/core';
import {ErrorNotificationComponent} from '@app/_common/notification/error-notification.component';
import {MatSnackBar} from '@angular/material/snack-bar';

@Injectable({providedIn: 'root'})
export class NotificationService {
  private readonly defaultDuration = 5000;
  
  constructor(private snackBarService: MatSnackBar) {
  }

  openError() {
    this.snackBarService.openFromComponent(ErrorNotificationComponent, {
      duration: this.defaultDuration,
      horizontalPosition: 'right',
      verticalPosition: 'top'
    });
  }
}
