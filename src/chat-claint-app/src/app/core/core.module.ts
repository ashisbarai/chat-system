import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptorService } from './auth-interceptor.service';
import { AuthService } from './auth-service.service';
import { AppconfigService } from '../appconfig.service';
import { AuthGuardService } from './auth-guard.service';

@NgModule({
    imports: [],
    exports: [],
    declarations: [],
    providers: [
        AppconfigService,
        AuthService,
        AuthGuardService,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true }
    ],
})
export class CoreModule { }
