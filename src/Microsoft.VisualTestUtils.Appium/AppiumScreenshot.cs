// Copyright (c) Bret Johnson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OpenQA.Selenium.Appium;

namespace Microsoft.VisualTestUtils.Appium
{
    /// <summary>
    /// Take screenshots using Appium.
    /// </summary>
    public class AppiumScreenshot : IScreenshot
    {
        private AppiumDriver driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumScreenshot"/> class.
        /// </summary>
        /// <param name="driver">The AppiumDriver to use for taking screenshots.</param>
        public AppiumScreenshot(AppiumDriver driver)
        {
            this.driver = driver;
        }

        /// <inheritdoc/>
        public bool TakeScreenshot(out byte[]? imageBytes)
        {
            imageBytes = this.driver.GetScreenshot().AsByteArray;
            return imageBytes != null && imageBytes.Any();
        }
    }
}
