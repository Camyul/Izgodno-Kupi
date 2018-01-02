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

paths.js = paths.webroot + "js/lightingTheme/**/*.js";
paths.siteJs = paths.webroot + "js/site.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.themeMinJs = paths.webroot + "js/lightingTheme/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.tempConcatJsDest = paths.webroot + "js/lightingTheme/temp.min.js";
paths.themeConcatJsDest = paths.webroot + "js/theme.min.js";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("cleanTheme:js", function (cb) {
    rimraf(paths.themeConcatJsDest, cb);
});

gulp.task("cleanTemp:js", function (cb) {
    rimraf(paths.tempConcatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "cleanTheme:js", "cleanTemp:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.themeMinJs], { base: "." })
        .pipe(concat(paths.tempConcatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("minSite:js", function () {
    return gulp.src([paths.siteJs, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("concatMin:js", function () {
    return gulp.src(paths.themeMinJs, { base: "." })
        .pipe(concat(paths.themeConcatJsDest))
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("minJs", ["min:js", "minSite:js"]);
gulp.task("concatJsAndMinCss", ["concatMin:js", "min:css"]);
gulp.task('default', ['concatJsAndMinCss']);
gulp.task('watch', function () {
    gulp.watch(paths.css, ['min:css']);
    gulp.watch(paths.js, ['min:js']);
    gulp.watch(paths.js, ['minSite:js']);
});