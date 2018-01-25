/// <binding BeforeBuild='clean' AfterBuild='minJs, concatJsAndMinCss' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var paths = {
    webroot: "./wwwroot/"
};

paths.css = paths.webroot + "css/**/*.css";

paths.lightingThemeCss = paths.webroot + "css/lightingTheme/**/*.css";
paths.modalCss = paths.webroot + "css/modal/**/*.css";
paths.paperAdminCss = paths.webroot + "css/paperAdminTheme/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";

paths.tempWebConcatMinCss = paths.webroot + "css/web-temp.min.css";
paths.tempModalConcatMinCss = paths.webroot + "css/modal-temp.min.css";
paths.tempAdminConcatMinCss = paths.webroot + "css/admin-temp.min.css";
paths.AdminAnimateMinCss = paths.webroot + "css/paperAdminTheme/animate.min.css";
paths.siteCss = paths.webroot + "css/site.css";

paths.concatWebCssDest = paths.webroot + "css/site.min.css";
paths.concatAdminCssDest = paths.webroot + "css/admin-site.min.css";

paths.js = paths.webroot + "js/lightingTheme/**/*.js";
paths.siteJs = paths.webroot + "js/site.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.themeMinJs = paths.webroot + "js/lightingTheme/**/*.min.js";

paths.adminJs = paths.webroot + "js/paperAdminTheme/**/*.js";
paths.adminMinJs = paths.webroot + "js/paperAdminTheme/**/*.min.js";
paths.adminThemeConcatJsDest = paths.webroot + "js/paperAdminTheme/adminTheme.min.js";

paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.tempConcatJsDest = paths.webroot + "js/lightingTheme/temp.min.js";
paths.themeConcatJsDest = paths.webroot + "js/theme.min.js";

//AdminTheme JS minify adminTheme.min.js
gulp.task("minAdmin:js", function () {
    return gulp.src([paths.adminJs, "!" + paths.adminMinJs], { base: "." })
        .pipe(concat(paths.adminThemeConcatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});
//Theme JS minify temp.min.js
gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.themeMinJs], { base: "." })
        .pipe(concat(paths.tempConcatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});
//Site JS minify site.min.js
gulp.task("minSite:js", function () {
    return gulp.src([paths.siteJs, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});
//Concat Theme MinJS Files theme.min.js
gulp.task("concatMin:js", function () {
    return gulp.src([paths.minJs, "!" + paths.concatJsDest, "!" + paths.adminThemeConcatJsDest])
        .pipe(concat(paths.themeConcatJsDest))
        .pipe(gulp.dest("."));
});
//gulp.task("concatMin:js", function () {
//    return gulp.src(paths.themeMinJs, { base: "." })
//        .pipe(concat(paths.themeConcatJsDest))
//        .pipe(gulp.dest("."));
//});

//Del site.min.js
gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});
//Del theme.min.js
gulp.task("cleanTheme:js", function (cb) {
    rimraf(paths.themeConcatJsDest, cb);
});
//Del adminTheme.min.js
gulp.task("cleanAdminTheme:js", function (cb) {
    rimraf(paths.adminThemeConcatJsDest, cb);
});
//Del lightingTheme/temp.min.js
gulp.task("cleanTemp:js", function (cb) {
    rimraf(paths.tempConcatJsDest, cb);
});

//Del web-temp.min.css
gulp.task("cleanWebTemp:css", function (cb) {
    rimraf(paths.tempWebConcatMinCss, cb);
});
//Del admin-temp.min.css
gulp.task("cleanAdminTemp:css", function (cb) {
    rimraf(paths.tempAdminConcatMinCss, cb);
});
//Del modal-temp.min.css
gulp.task("cleanModalTemp:css", function (cb) {
    rimraf(paths.tempModalConcatMinCss, cb);
});
//Del site.min.css
gulp.task("cleanSiteCss:css", function (cb) {
    rimraf(paths.concatWebCssDest, cb);
});
//Del admin-site.min.css
gulp.task("cleanAdminSiteCss:css", function (cb) {
    rimraf(paths.concatAdminCssDest, cb);
});

gulp.task("clean", ["clean:js", "cleanTheme:js", "cleanTemp:js", "cleanAdminTheme:js",
                    "cleanWebTemp:css", "cleanAdminTemp:css", "cleanModalTemp:css", "cleanSiteCss:css", "cleanAdminSiteCss:css"]);

//Concat and Minify all Css web-temp.min.css
gulp.task("minLightTheme:css", function () {
    return gulp.src([paths.lightingThemeCss, "!" + paths.minCss])
        .pipe(concat(paths.tempWebConcatMinCss))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});
//Concat and Minify all Css modal-temp.min.css
gulp.task("minModal:css", function () {
    return gulp.src([paths.modalCss, paths.siteCss, "!" + paths.minCss])
        .pipe(concat(paths.tempModalConcatMinCss))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});
//Concat and Minify all Css admin-temp.min.css
gulp.task("minPaperAdmin:css", function () {
    return gulp.src([paths.paperAdminCss, paths.siteCss, "!" + paths.minCss])
        .pipe(concat(paths.tempAdminConcatMinCss))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

//Waiting "minLightTheme:css, minModal:css" to completed and create site.min.css
gulp.task("concatWebMinCss:css", ["minLightTheme:css", "minModal:css"], function () {
    return gulp.src([paths.tempWebConcatMinCss, paths.tempModalConcatMinCss])
        .pipe(concat(paths.concatWebCssDest))
        .pipe(gulp.dest("."));
});

//Waiting "minPaperAdmin:css" to completed and create admin-site.min.css
gulp.task("concatAdminMinCss:css", ["minPaperAdmin:css"], function () {
    return gulp.src([paths.tempAdminConcatMinCss, paths.AdminAnimateMinCss])
        .pipe(concat(paths.concatAdminCssDest))
        .pipe(gulp.dest("."));
});


gulp.task("minCss", ["concatWebMinCss:css", "concatAdminMinCss:css"]);
gulp.task("minJs", ["min:js", "minAdmin:js", "minSite:js"]);
gulp.task("concatJsAndMinCss", ["concatMin:js", "minCss"]);
gulp.task('default', ['concatJsAndMinCss']);
gulp.task('watch', function () {
    gulp.watch(paths.css, ['min:css']);
    gulp.watch(paths.js, ['min:js']);
    gulp.watch(paths.js, ['minSite:js']);
});