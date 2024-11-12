# Documentation for Backend + API

## Setup

- open ./Python in Powershell
- ``` ./Setup.ps1 ```
- ``` python DevDbSetup.py ```


## Testing

- open ./Python in Powershell
- ``` ./Setup.ps1 ```
- ``` python Backend.py ```

## API Paths
<details>
  <summary>
    Leaderboard
  </summary>
    <code>http://localhost:5000/api/Leaderboard{amount}</code><br>
    <pre>
      <code>
        [
          {
            "MaxDepth": 1000.0,
            "UserName": "RayVenTheManta"
          },
          {
            "MaxDepth": 360.0,
            "UserName": "Philsie"
          }
        ]
      </code>
    </pre>
</details>

<details>
  <summary>
    Player Data
  </summary>
    <code>http://localhost:5000/api/getAllUsers</code><br>
    <pre>
      <code>
        [
          {
            "DailyDepth": 420,
            "MaxDepth": 1000,
            "Tier": 42,
            "UUID": 0,
            "Upgrades": {
              "Speed": 1.5,
              "Stamina": 100
            },
            "UserName": "RayVenTheManta"
          },
          {
            "DailyDepth": 0,
            "MaxDepth": 360,
            "Tier": 9,
            "UUID": 1,
            "Upgrades": {
              "Speed": 1.2,
              "Stamina": 120
            },
            "UserName": "Philsie"
          }
        ]
      </code>
    </pre>
</details>
