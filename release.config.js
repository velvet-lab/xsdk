/**
 * @type {import('semantic-release').GlobalConfig}
 */

// define here all assets which should included in a release
let assets = [];

let plugins = [
  [
    '@semantic-release/commit-analyzer',
    {
      preset: 'conventionalcommits',
    },
  ],
  [
    '@semantic-release/release-notes-generator',
    {
      preset: 'conventionalcommits',
    },
  ],
  [
    '@semantic-release/changelog',
    {
      changelogFile: 'CHANGELOG.md',
    },
  ],
  [
    '@semantic-release/npm',
    {
      npmPublish: false,
    },
  ],
  [
    '@semantic-release/git',
    {
      assets: ['package.json', 'CHANGELOG.md'],
      message: 'chore(release): ${nextRelease.version} [skip ci] [skip release]\n\n${nextRelease.notes}',
    },
  ],
];

if (!process.env.NPM_TOKEN) {
  // Token is not needed because we do not publish to a npm registry
  process.env.NPM_TOKEN = 'adummytoken';
}

if (process.env.CI === 'true') {
  plugins = [
    ...plugins,
    [
      '@semantic-release/github',
      {
        assets: [...assets],
      },
    ],
  ];
}

module.exports = {
  branches: [
    {
      name: 'next',
      channel: 'next',
      prerelease: 'next',
    },
    {
      name: 'rc',
      channel: 'rc',
      prerelease: 'rc',
    },
    {
      name: 'main',
      channel: 'stable',
    },
  ],
  ci: true,
  plugins: [...plugins],
};
