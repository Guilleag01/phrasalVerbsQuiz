import requests
from bs4 import BeautifulSoup
import json
import re

def main():
    page = requests.get('https://skypeenglishclasses.com/english-phrasal-verbs').text
    # with open('test.html', 'w') as file:
    #     file.write(page)
    soup = BeautifulSoup(page, features="lxml")

    db:dict = {}

    for t in soup.body.find_all('tr')[1:]:
        td = t.find_all('td')[0]
        # print(td[0].find_all('a'))
        a = td.find_all('a');
        phVerb:str = ""
        if len(a) > 0:
            print(a[0].text)
            phVerb = a[0].text.lower()
        else:
            print(td.text)
            phVerb = td.text.lower()

        examples:dict = getExamples(phVerb)
        db[phVerb.replace(" ", "-")] = examples
        with open("db.json", "w") as jfile:
            json.dump(db, jfile, indent=4, ensure_ascii=False)

        

def getExamples(phVerb: str) -> dict:
    examplesPage = requests.get('https://skypeenglishclasses.com/english-phrasal-verbs/' + phVerb.replace(" ", "-").replace("/", "")).text
    with open('test.html', 'w') as file:
        file.write(examplesPage)

    soup = BeautifulSoup(examplesPage, features="lxml")

    examples: list = []
    div = soup.body.find_all('div', attrs={'class':'text2'})
    if len(div) > 0:
        ps = div[-1].find_all('p')[1:-1:2]
        if len(ps) > 0:
            for p in ps:
                examples += re.sub(r"(Examples: |</?p>|\n|(\[[a-zA-Z]*\]))", "", str(p)).split("<br/>")

    return {
        "pv": phVerb, 
        "words": phVerb.split(" "), 
        "examples": examples
    }
        


if __name__ == "__main__":
    main()