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
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.tempConcatMinCss = paths.webroot + "css/temp.min.css";
paths.concatCssDest = paths.webroot + "css/site.min.css";

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
    return gulp.src([paths.minJs, "!" + paths.concatJsDest])
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
//Del site.min.css
gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});
//Del temp.min.css
gulp.task("cleanTemp:css", function (cb) {
    rimraf(paths.tempConcatMinCss, cb);
});

gulp.task("clean", ["clean:js", "cleanTheme:js", "cleanTemp:js", "clean:css", "cleanTemp:css", "cleanAdminTheme:js"]);

paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.tempConcatMinCss = paths.webroot + "css/temp.min.css";
paths.concatCssDest = paths.webroot + "css/site.min.css";

//Concat and Minify all Css temp.min.css
gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.tempConcatMinCss))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

//Waiting "min:css" to completed and run
gulp.task("concatMinCss:css", ["min:css"], function () {
    return gulp.src([paths.minCss, "!" + paths.concatCssDest])
        .pipe(concat(paths.concatCssDest))
        .pipe(gulp.dest("."));
});

gulp.task("minCss", ["concatMinCss:css"]);
gulp.task("minJs", ["min:js", "minAdmin:js", "minSite:js"]);
gulp.task("concatJsAndMinCss", ["concatMin:js", "minCss"]);
gulp.task('default', ['concatJsAndMinCss']);
gulp.task('watch', function () {
    gulp.watch(paths.css, ['min:css']);
    gulp.watch(paths.js, ['min:js']);
    gulp.watch(paths.js, ['minSite:js']);
});