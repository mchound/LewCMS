module.exports = function (grunt) {

    // Project configuration.
    grunt.initConfig({
        less: {

            development: {
                options: {
                    paths: ["styles"]
                },
                files: {
                    "styles/build/site.css": "styles/src/site.less"
                },
                compress: false
            }

        },
        watch: {
            less: {
                files: "styles/**/*.less",
                tasks: ['less']
            },
            react: {
                files: "javascripts/src/ReactComponents/**/*.jsx",
                tasks: ['task-react']
            }
            
        },
        react: {
            files: {
                expand: true,
                cwd: 'javascripts/src/ReactComponents',
                src: ['**/*.jsx'],
                dest: 'javascripts/build/ReactComponents',
                ext: '.js'
            }
        }

    });

    // Load the plugin that provides the "uglify" task.
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-react');

    // Default task(s).
    grunt.registerTask('auto', ['watch']);
    grunt.registerTask('auto-less', ['watch:less']);
    grunt.registerTask('auto-react', ['watch:react']);
    grunt.registerTask('default', ['less', 'react']);
    grunt.registerTask('task-less', ['less']);
    grunt.registerTask('task-react', ['react']);

};