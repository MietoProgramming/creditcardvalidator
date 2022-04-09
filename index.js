const puppeteer = require('puppeteer');

const main = async () => {
    const browser = await puppeteer.launch(
        { // Gui ? true : false
            headless: false
        }
    );
    const page = await browser.newPage();
    page.on('console', (msg) => console.log('PAGE LOG:', msg.text()));

    await page.goto('https://www.oxfordlearnersdictionaries.com/wordlists/oxford3000-5000');
    await page.waitForSelector("#onetrust-accept-btn-handler");
    console.log("modal");
    await page.evaluate(() => {
        document.querySelector("#onetrust-accept-btn-handler").click();
    });
    console.log("clicked")
    await page.waitForSelector('#wordlistsContentPanel .top-g');
    console.log("wordlist");
    const getData = await page.evaluate(() => {
        // Button click
        // document.querySelector('button[name="Name"]').click();

        // Fill input with 'Login' value
        // document.querySelector('input[name="login"]', input => input.value = 'Login');


        // page.waitForSelector('#wordlistsContentPanel .top-g');
        // Get all p tags
        const pTags = [...document.querySelectorAll('#wordlistsContentPanel .top-g li')].map(tag => {
            return {
                name: tag.querySelector("a").innerText,
                description: tag.querySelector("span").innerText,
                level: tag.querySelector(".belong-to")?.innerText
            }
        });
        // console.log(pTags);
        return pTags;
        // console.log(pTags);
        // return pTags;

        //Wait for delayed element
        // await page.waitForSelector('.delayedBox');
    })

    console.log(getData);

    // Close browser
    // await browser.close();
}

main();