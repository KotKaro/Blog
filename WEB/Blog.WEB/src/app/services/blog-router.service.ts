import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class BlogRouterService {
    currentUser;

    constructor(private router: Router) {
    }

    goToBlog(): void {
        this.router.navigate(['/blog']);
    }

    goToPostDetails(id: string): void {
        if (!id) {
            this.goToBlog();
        }

        this.router.navigate(['/post', id]);
    }

    goToPostCreate(): void {
        this.router.navigate(['/post-create']);
    }

    goToEditPost(id: string): void {
        if (!id) {
            return this.goToBlog();
        }

        this.router.navigate(['/post/edit/', id]);
    }

    goToLogin(): void {
        this.router.navigate(['/login']);
    }
}