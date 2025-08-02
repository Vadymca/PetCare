plugins {
    alias(libs.plugins.android.application)
}

android {
    namespace = "com.example.petcareapp"
    compileSdk = 35

    defaultConfig {
        applicationId = "com.example.petcareapp"
        minSdk = 24
        targetSdk = 35
        versionCode = 1
        versionName = "1.0"
        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildFeatures {
        viewBinding = true
        dataBinding = true
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }

    lint {
        abortOnError = true               // Зупинити збірку при помилках
        warningsAsErrors = true           // Всі попередження як помилки
        checkReleaseBuilds = true         // Перевіряти релізні збірки
        xmlReport = true                  // Звіт у форматі XML
        htmlReport = true                 // Звіт у HTML
        htmlOutput = file("$buildDir/reports/lint-report.html")
        baseline = file("lint-baseline.xml") // Додамо пізніше
    }

}

dependencies {
    implementation(libs.appcompat)
    implementation(libs.material)
    implementation(libs.activity)
    implementation(libs.constraintlayout)
    implementation(libs.dagger)
    implementation(libs.navigation.fragment)
    implementation(libs.support.annotations)
    implementation(libs.annotation)
    annotationProcessor(libs.dagger.compiler) // Для генерації Dagger коду
    implementation(libs.retrofit)
    implementation(libs.retrofit.gson)
    implementation(libs.okhttp)
    implementation(libs.room.runtime)
    annotationProcessor(libs.room.compiler) // Для Room
    implementation(libs.glide)
    annotationProcessor(libs.glide.compiler)
    implementation(libs.fragment.ktx)
    implementation(libs.okhttp3.logging.interceptor)
    testImplementation(libs.junit)
    androidTestImplementation(libs.ext.junit)
    androidTestImplementation(libs.espresso.core)
}