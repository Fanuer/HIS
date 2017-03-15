import { OAuthService } from 'angular-oauth2-oidc';
import { Component } from '@angular/core';

@Component({
    selector: 'navigation',
    templateUrl: 'navigation.component.html'
})

export class NavigationComponent {
    constructor(private oauthService: OAuthService) {}

    login() {
        this.oauthService.initImplicitFlow();
    }

    logout() {
        this.oauthService.logOut();
    }

    get Username() {
        let claims = this.oauthService.getIdentityClaims();
        if (!claims) {return null; }
        return claims.given_name;
    }
}